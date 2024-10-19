using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.DAL.Cache;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class AuthService(IMapper mapper, ITokenService tokenService, IUnitOfWork unitOfWork,
            IAuthValidator authValidator, IPasswordHasher passwordHasher, ICacheService cacheService) : IAuthService
    {

        /// <inheritdoc/>
        public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
        {
            var user = await unitOfWork.Users.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == dto.Login);

            var validateLoginResult = authValidator.ValidateLogin(user, enteredPassword: dto.Password);
            if (!validateLoginResult.IsSuccess)
            {
                return BaseResult<TokenDto>.Failure((int)validateLoginResult.Error.Code, validateLoginResult.Error.Message);
            }

            var claims = tokenService.GetClaimsFromUser(user);

            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();

            var userToken = await unitOfWork.UserTokens.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (userToken == null)
            {
                userToken = new UserToken()
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7)
                };

                await unitOfWork.UserTokens.CreateAsync(userToken);
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

                unitOfWork.UserTokens.Update(userToken);
            }

            await unitOfWork.SaveChangesAsync();

            return BaseResult<TokenDto>.Success(new TokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            });
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
        {
            if (dto.Password != dto.PasswordConfirm)
            {
                return BaseResult<UserDto>.Failure((int)ErrorCodes.PasswordNotEqualsPasswordConfirm, ErrorMessage.PasswordNotEqualsPasswordConfirm);
            }

            var user = await unitOfWork.Users.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);

            if (user != null)
            {
                return BaseResult<UserDto>.Failure((int)ErrorCodes.UserAlreadyExists, ErrorMessage.UserAlreadyExists);
            }

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    user = new User()
                    {
                        Login = dto.Login,
                        Password = passwordHasher.Hash(dto.Password),
                    };
                    await unitOfWork.Users.CreateAsync(user);

                    await unitOfWork.SaveChangesAsync();

                    var dateOfBirth = dto.DateOfBirth;

                    UserProfile userProfile = new UserProfile()
                    {
                        UserId = user.Id,
                        IsEditAble = true,
                        Name = dto.Name,
                        Surname = dto.Surname,
                        Age = dateOfBirth.GetYearsByDateToNow(),
                        DateOfBirth = dateOfBirth,
                        IsExamCompleted = false,
                        CurrentGrade = 0,
                        LessonsCompleted = 0,
                        CountOfReviews = 0
                    };

                    await unitOfWork.UserProfiles.CreateAsync(userProfile);

                    var role = await unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Name == nameof(Roles.User));

                    if (role == null)
                    {
                        return BaseResult<UserDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
                    }

                    UserRole userRole = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    };

                    await unitOfWork.UserRoles.CreateAsync(userRole);

                    await unitOfWork.SaveChangesAsync();

                    await cacheService.RemoveAsync(CacheKeys.Users);

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }

            return BaseResult<UserDto>.Success(mapper.Map<UserDto>(user));
        }
    }
}
