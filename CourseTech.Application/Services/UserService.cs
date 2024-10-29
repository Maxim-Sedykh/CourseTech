using AutoMapper;
using CourseTech.Application.Commands.UserCommand;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Commands.UserTokenCommands;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Queries.UserTokenQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class UserService(IUnitOfWork unitOfWork, ICacheService cacheService, IMediator mediator) : IUserService
    {
        public async Task<BaseResult> DeleteUserAsync(Guid userId)
        {
            var user = await mediator.Send(new GetUserByIdQuery(userId));

            if (user is null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            var userProfile = await mediator.Send(new GetProfileByUserIdQuery(userId));

            if (userProfile is null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var userToken = await mediator.Send(new GetUserTokenByUserIdQuery(userId));

            using (var transaction = await unitOfWork.BeginTransactionAsync())
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
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return BaseResult.Success();
        }

        public async Task<BaseResult<UpdateUserDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await mediator.Send(new GetUpdateUserDtoByUserIdQuery(userId));

            if (user is null)
            {
                return BaseResult<UpdateUserDto>.Failure((int)ErrorCodes.UserNotFound, ErrorMessage.UserNotFound);
            }

            return BaseResult<UpdateUserDto>.Success(user);
        }

        public async Task<CollectionResult<UserDto>> GetUsersAsync()
        {
            var users = await cacheService.GetOrAddToCache(
                CacheKeys.Users,
                async () => await mediator.Send(new GetUserDtosQuery()));

            return CollectionResult<UserDto>.Success(users);
        }

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
