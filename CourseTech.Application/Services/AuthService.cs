using AutoMapper;
using CourseTech.Application.Commands.UserCommands;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Domain;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Data;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services;

public class AuthService(
        IUserTokenRepository userTokenRepository,
        IUserRepository userRepository,
        IMediator mediator,
        IMapper mapper,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IAuthValidator authValidator,
        ICacheService cacheService,
        ILogger logger,
        IOptions<JwtSettings> jwtOptions
    ) : IAuthService
{
    public JwtSettings JwtSettings { get; } = jwtOptions.Value;


    /// <inheritdoc/>
    public async Task<Result<TokenDto>> Login(LoginUserDto dto)
    {
        var user = await userRepository.GetByLoginAsync(dto.Login);

        var validateLoginResult = authValidator.ValidateLogin(user, enteredPassword: dto.Password);
        if (!validateLoginResult.IsSuccess)
        {
            return Result.Failure<TokenDto>(validateLoginResult.Errors);
        }

        var claims = tokenService.GetClaimsFromUser(user);

        var accessToken = tokenService.GenerateAccessToken(claims);
        var refreshToken = tokenService.GenerateRefreshToken();

        var userToken = await userTokenRepository.GetByUserIdAsync(user.Id);

        if (userToken == null)
        {
            await mediator.Send(new CreateUserTokenCommand(user.Id, refreshToken, JwtSettings.RefreshTokenValidityInDays));
        }
        else
        {
            await mediator.Send(new UpdateUserTokenCommand(userToken, refreshToken));
        }

        return Result.Success(new TokenDto()
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
        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result<UserDto>> Register(RegisterUserDto dto)
    {
        var user = await userRepository.GetByLoginAsync(dto.Login);

        var validateRegisterResult = authValidator.ValidateRegister(user, enteredPassword: dto.Password, enteredPasswordConfirm: dto.PasswordConfirm);
        if (!validateRegisterResult.IsSuccess)
        {
            return Result.Failure<UserDto>(validateRegisterResult.Errors);
        }

        using (var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead))
        {
            try
            {
                user = await mediator.Send(new CreateUserCommand(dto.Login, dto.Password));

                await mediator.Send(new CreateUserProfileCommand(user.Id, dto.DateOfBirth));

                await unitOfWork.SaveChangesAsync();

                await cacheService.RemoveAsync(CacheKeys.Users);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                logger.Error(ex, ex.Message);
                return Result<UserDto>.Failure($"Error while registrating user with message {ex.Message}");
            }
        }

        return Result.Success(mapper.Map<UserDto>(user));
    }
}
