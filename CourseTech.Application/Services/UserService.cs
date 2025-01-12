using CourseTech.Application.Commands.UserCommand;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Application.Queries.Dtos.UserDtoQueries;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.Application.Queries.Entities.UserQueries;
using CourseTech.Application.Queries.Entities.UserTokenQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Interfaces.Validators;
using CourseTech.Domain.Result;
using MediatR;
using System.Data;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services
{
    public class UserService(
        ICacheService cacheService,
        IMediator mediator,
        IUnitOfWork unitOfWork,
        ILogger logger,
        IUserValidator userValidator) : IUserService
    {
        /// <inheritdoc/>
        public async Task<BaseResult> DeleteUserAsync(Guid userId)
        {
            var user = await mediator.Send(new GetUserByIdQuery(userId));
            var userProfile = await mediator.Send(new GetProfileByUserIdQuery(userId));

            var validationResult = userValidator.ValidateDeletingUser(userProfile, user);
            if (!validationResult.IsSuccess)
            {
                return BaseResult.Failure((int)validationResult.Error.Code, validationResult.Error.Message);
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

                    return BaseResult.Failure((int)ErrorCodes.DeleteUserFailed, ErrorMessage.DeleteUserFailed);
                }
            }

            return BaseResult.Success();
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UpdateUserDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await mediator.Send(new GetUpdateUserDtoByUserIdQuery(userId));

            if (user is null)
            {
                return BaseResult<UpdateUserDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            return BaseResult<UpdateUserDto>.Success(user);
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
        public async Task<BaseResult<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto)
        {
            var user = await mediator.Send(new GetUserWithProfileByUserIdQuery(dto.Id));

            if (user is null)
            {
                return BaseResult<UpdateUserDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            await mediator.Send(new UpdateUserCommand(dto, user));

            await cacheService.RemoveAsync(CacheKeys.Users);

            return BaseResult<UpdateUserDto>.Success(dto);
        }
    }
}
