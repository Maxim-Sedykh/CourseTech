using CourseTech.Application.Commands.UserCommands;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Domain;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services;

public class UserService(
    ICacheService cacheService,
    IUserRepository userRepository,
    IUserTokenRepository userTokenRepository,
    IMediator mediator,
    IUnitOfWork unitOfWork,
    ILogger logger,
    IUserValidator userValidator) : IUserService
{
    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        var user = await userRepository.GetUserWithProfileById(userId);

        var validationResult = userValidator.ValidateDeletingUser(user);
        if (!validationResult.IsSuccess)
        {
            return Result.Failure(validationResult.Errors);
        }

        var userToken = await userTokenRepository.GetByUserIdAsync(userId);

        using (var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead))
        {
            try
            {
                await mediator.Send(new DeleteUserCommand(user));
                await mediator.Send(new DeleteUserProfileCommand(user.UserProfile));

                if (userToken != null)
                {
                    await mediator.Send(new DeleteUserTokenCommand(userToken));
                }

                await unitOfWork.SaveChangesAsync();

                await cacheService.RemoveAsync(CacheKeys.Users);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);

                await transaction.RollbackAsync();

                return Result.Failure("Transaction of deleting user is failed");
            }
        }

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result<UpdateUserDto>> GetUserByIdAsync(Guid userId)
    {
        var user = new UpdateUserDto(); //TODO заглушечка

        if (user is null)
        {
            return Result<UpdateUserDto>.Failure("User not found");
        }

        return Result.Success(user);
    }

    /// <inheritdoc/>
    public async Task<Result<UserDto[]>> GetUsersAsync()
    {
        var users = await cacheService.GetOrAddToCache(
            CacheKeys.Users,
            async () => await userRepository.GetAll().ToListAsync());

        return Result<UserDto>.Success(users.Select(x => new UserDto()).ToArray()); //TODO заглушечка
    }

    /// <inheritdoc/>
    public async Task<Result<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto)
    {
        var user = await userRepository.GetUserWithProfileById(dto.Id);

        if (user is null)
        {
            return Result<UpdateUserDto>.Failure("User not found");
        }

        await mediator.Send(new UpdateUserCommand(dto, user));

        await cacheService.RemoveAsync(CacheKeys.Users);

        return Result.Success(dto);
    }
}
