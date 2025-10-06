using AutoMapper;
using Azure.Core;
using CourseTech.DAL.Repositories;
using CourseTech.Domain;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Helpers;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Data;
using System.Threading;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services;

public class AuthService(
        IUserTokenRepository userTokenRepository,
        IUserRepository userRepository,
        IUserProfileRepository userProfileRepository,
        IMapper mapper,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IAuthValidator authValidator,
        ICacheService cacheService,
        IPasswordHasher passwordHasher,
        ILogger logger,
        IOptions<JwtSettings> jwtOptions) : IAuthService
{

    public JwtSettings JwtSettings { get; } = jwtOptions.Value;


    /// <inheritdoc/>
    public async Task<Result<TokenDto>> Login(LoginUserDto dto)
    {
        var user = await userRepository.GetByLoginAsync(dto.Login);

        var validateLoginResult = authValidator.ValidateLogin(user, enteredPassword: dto.Password);
        if (!validateLoginResult.Success)
        {
            return Result.Error<TokenDto>(validateLoginResult.Errors);
        }

        var claims = tokenService.GetClaimsFromUser(user);

        var accessToken = tokenService.GenerateAccessToken(claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        var userToken = await userTokenRepository.GetByUserIdAsync(user.Id);

        if (userToken == null)
        {
            var createdUserToken = new UserToken()
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                RefreshTokenExpireTime = DateTime.UtcNow.AddDays(JwtSettings.RefreshTokenValidityInDays)
            };

            await userTokenRepository.CreateAsync(createdUserToken);

            await userTokenRepository.SaveChangesAsync();
        }
        else
        {
            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

            userTokenRepository.Update(userToken);

            await userTokenRepository.SaveChangesAsync();
        }

        return DataResult<TokenDto>.Success(new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    public async Task<Result> LogoutAsync(Guid userId)
    {
        var userToken = await userTokenRepository.GetByUserIdAsync(userId);
        if (userToken != null)
        {
            userTokenRepository.Remove(userToken);

            await userTokenRepository.SaveChangesAsync();
        }
        return Result.Ok();
    }

    /// <inheritdoc/>
    public async Task<Result<UserDto>> Register(RegisterUserDto dto)
    {
        var user = await userRepository.GetByLoginAsync(dto.Login);

        var validateRegisterResult = authValidator.ValidateRegister(user, enteredPassword: dto.Password, enteredPasswordConfirm: dto.PasswordConfirm);
        if (!validateRegisterResult.Success)
        {
            return Result.Error<UserDto>(validateRegisterResult.Errors);
        }

        using (var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead))
        {
            try
            {
                var createdUser = new User()
                {
                    Login = dto.Login,
                    Password = passwordHasher.Hash(dto.Password),
                };

                await userRepository.CreateAsync(user);

                await userRepository.SaveChangesAsync();

                var userProfile = new UserProfile()
                {
                    UserId = createdUser.Id,
                    Age = dto.DateOfBirth.GetYearsByDateToNow(),
                    DateOfBirth = dto.DateOfBirth,
                    IsExamCompleted = false,
                    LessonsCompleted = 0
                };

                await userProfileRepository.CreateAsync(userProfile);

                var role = await mediator.Send(new GetRoleByNameQuery(nameof(Roles.User)));

                if (role == null)
                {
                    return DataResult<UserDto>.Failure((int)ErrorCode.RoleNotFound, ErrorMessage.RoleNotFound);
                }

                await mediator.Send(new CreateUserRoleCommand(role.Id, user.Id));

                await unitOfWork.SaveChangesAsync();

                await cacheService.RemoveAsync(CacheKeys.Users);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                logger.Error(ex, ex.Message);
                return DataResult<UserDto>.Failure($"Error while registrating user with message {ex.Message}");
            }
        }

        return DataResult<UserDto>.Success(mapper.Map<UserDto>(user));
    }
}
