using AutoMapper;
using CourseTech.Application.CQRS.Commands.RoleCommands;
using CourseTech.Application.CQRS.Commands.UserCommand;
using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Commands.UserTokenCommands;
using CourseTech.Application.CQRS.Queries.Entities.RoleQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserQueries;
using CourseTech.Application.CQRS.Queries.Entities.UserTokenQueries;
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
using Microsoft.Extensions.Options;
using System.Data;
using ILogger = Serilog.ILogger;
using Roles = CourseTech.Domain.Constants.Roles;

namespace CourseTech.Application.Services;

/// <summary>
/// Сервис для аутентификации и авторизации
/// </summary>
/// <param name="mediator"></param>
/// <param name="mapper"></param>
/// <param name="tokenService"></param>
/// <param name="unitOfWork"></param>
/// <param name="authValidator"></param>
/// <param name="cacheService"></param>
/// <param name="logger"></param>
/// <param name="jwtOptions"></param>
public class AuthService(
        IMediator mediator,
        IMapper mapper,
        ITokenService tokenService,
        ITransactionManager unitOfWork,
        IAuthValidator authValidator,
        ICacheService cacheService,
        ILogger logger,
        IOptions<JwtSettings> jwtOptions) : IAuthService
{

    /// <inheritdoc/>
    public async Task<DataResult<TokenDto>> Login(LoginUserDto dto)
    {
        var user = await mediator.Send(new GetUserWithRolesByLoginQuery(dto.Login));

        var validateLoginResult = authValidator.ValidateLogin(user, enteredPassword: dto.Password);
        if (!validateLoginResult.IsSuccess)
        {
            return DataResult<TokenDto>.Failure((int)validateLoginResult.Error.Code, validateLoginResult.Error.Message);
        }

        var claims = tokenService.GetClaimsFromUser(user);

        var accessToken = tokenService.GenerateAccessToken(claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        var userToken = await mediator.Send(new GetUserTokenByUserIdQuery(user.Id));

        if (userToken == null)
        {
            await mediator.Send(new CreateUserTokenCommand(user.Id, refreshToken, jwtOptions.Value.RefreshTokenValidityInDays));
        }
        else
        {
            await mediator.Send(new UpdateUserTokenCommand(userToken, refreshToken));
        }

        return DataResult<TokenDto>.Success(new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    /// <inheritdoc/>
    public async Task<DataResult<UserDto>> Register(RegisterUserDto dto)
    {
        var user = await mediator.Send(new GetUserByLoginQuery(dto.Login));

        var validateRegisterResult = authValidator.ValidateRegister(user, enteredPassword: dto.Password, enteredPasswordConfirm: dto.PasswordConfirm);
        if (!validateRegisterResult.IsSuccess)
        {
            return DataResult<UserDto>.Failure((int)validateRegisterResult.Error.Code, validateRegisterResult.Error.Message);
        }

        using (var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead))
        {
            try
            {
                user = await mediator.Send(new CreateUserCommand(dto.Login, dto.Password));

                await mediator.Send(new CreateUserProfileCommand(user.Id, dto.UserName, dto.Surname, dto.DateOfBirth));

                var role = await mediator.Send(new GetRoleByNameQuery(nameof(Roles.User)));

                if (role == null)
                {
                    return DataResult<UserDto>.Failure((int)ErrorCodes.RoleNotFound, ErrorMessage.RoleNotFound);
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
                return DataResult<UserDto>.Failure((int)ErrorCodes.RegistrationFailed, ErrorMessage.RegistrationFailed);
            }
        }

        return DataResult<UserDto>.Success(mapper.Map<UserDto>(user));
    }
}
