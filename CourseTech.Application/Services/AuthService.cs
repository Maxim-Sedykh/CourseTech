using AutoMapper;
using CourseTech.Application.Commands.RoleCommands;
using CourseTech.Application.Commands.UserCommand;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Application.Queries.RoleQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Queries.UserTokenQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using CourseTech.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ILogger = Serilog.ILogger;
using Roles = CourseTech.Domain.Constants.Roles;

namespace CourseTech.Application.Services
{
    public class AuthService(
            IMapper mapper,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IAuthValidator authValidator,
            ICacheService cacheService,
            IMediator mediator,
            ILogger logger,
            IOptions<JwtSettings> jwtOptions) : IAuthService
    {

        public JwtSettings JwtSettings { get; } = jwtOptions.Value;


        /// <inheritdoc/>
        public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
        {
            var user = await mediator.Send(new GetUserWithRolesByLoginQuery(dto.Login));

            var validateLoginResult = authValidator.ValidateLogin(user, enteredPassword: dto.Password);
            if (!validateLoginResult.IsSuccess)
            {
                return BaseResult<TokenDto>.Failure((int)validateLoginResult.Error.Code, validateLoginResult.Error.Message);
            }

            var claims = tokenService.GetClaimsFromUser(user);

            var accessToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();

            var userToken = await mediator.Send(new GetUserTokenByUserIdQuery(user.Id));

            if (userToken == null)
            {
                await mediator.Send(new CreateUserTokenCommand(user.Id, refreshToken, JwtSettings.RefreshTokenValidityInDays));
            }
            else
            {
                await mediator.Send(new UpdateUserTokenCommand(userToken, refreshToken));
            }

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

            var user = await mediator.Send(new GetUserByLoginQuery(dto.Login));

            if (user != null)
            {
                return BaseResult<UserDto>.Failure((int)ErrorCodes.UserAlreadyExists, ErrorMessage.UserAlreadyExists);
            }

            using (var transaction = await unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    user = await mediator.Send(new CreateUserCommand(dto.Login, dto.Password));

                    await mediator.Send(new CreateUserProfileCommand(user.Id, dto.Name, dto.Surname, dto.DateOfBirth));

                    var role = await mediator.Send(new GetRoleByNameQuery(nameof(Roles.User)));

                    if (role == null)
                    {
                        return BaseResult<UserDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
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
                    return BaseResult<UserDto>.Failure((int)ErrorCodes.RegistrationFailed, ErrorMessage.RegistrationFailed);
                }
            }

            return BaseResult<UserDto>.Success(mapper.Map<UserDto>(user));
        }
    }
}
