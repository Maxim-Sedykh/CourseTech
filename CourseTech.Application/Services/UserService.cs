using CourseTech.Application.Resources;
using CourseTech.Domain;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using MediatR;
using System.Data;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services;

public class UserService(
    ICacheService cacheService,
    IUserRepository userRepository,
    IUserProfileRepository userProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger logger,
    IUserValidator userValidator) : IUserService
{
    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        var user = await userRepository.GetByIdAsync(userId);
        var userProfile = await userProfileRepository.GetByIdAsync(user);

        var validationResult = userValidator.ValidateDeletingUser(userProfile, user);
        if (!validationResult.IsSuccess)
        {
            return Result.Failure((int)validationResult.Error.Code, validationResult.Error.Message);
        }

        var userToken = await mediator.Send(new GetUserTokenByUserIdQuery(userId));

        using (var transaction = await unitOfWork.BeginTransactionAsync(IsolationLevel.RepeatableRead))
        {
            try
            {
                await mediator.Send(new DeleteUserCommand(user));
                await mediator.Send(new DeleteUserProfileCommand(userProfile));

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

                return Result.Failure((int)ErrorCode.DeleteUserFailed, ErrorMessage.DeleteUserFailed);
            }
        }

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<DataResult<UpdateUserDto>> GetUserByIdAsync(Guid userId)
    {
        var user = await mediator.Send(new GetUpdateUserDtoByUserIdQuery(userId));

        if (user is null)
        {
            return DataResult<UpdateUserDto>.Failure((int)ErrorCode.UserNotFound, ErrorMessage.UserNotFound);
        }

        return DataResult<UpdateUserDto>.Success(user);
    }

    /// <inheritdoc/>
    public async Task<CollectionResult<UserDto>> GetUsersAsync()
    {
        var users = await cacheService.GetOrAddToCache(
            CacheKeys.Users,
            async () => await mediator.Send(new GetUserDtosQuery()));

        return CollectionResult<UserDto>.Success(users);
    }

    /// <inheritdoc/>
    public async Task<DataResult<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto)
    {
        var user = await mediator.Send(new GetUserWithProfileByUserIdQuery(dto.Id));

        if (user is null)
        {
            return DataResult<UpdateUserDto>.Failure((int)ErrorCode.UserNotFound, ErrorMessage.UserNotFound);
        }

        await mediator.Send(new UpdateUserCommand(dto, user));

        await cacheService.RemoveAsync(CacheKeys.Users);

        return DataResult<UpdateUserDto>.Success(dto);
    }
}
