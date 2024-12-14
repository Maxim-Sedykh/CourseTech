using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.Dtos.UserProfileDtoQuery;
using CourseTech.Application.Queries.Entities.UserProfileQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;
using StackExchange.Redis;
using ILogger = Serilog.ILogger;

namespace CourseTech.Application.Services
{
    public class UserProfileService(
        ICacheService cacheService,
        IMediator mediator,
        IDatabase redisDatabase,
        ILogger logger) : IUserProfileService
    {
        /// <inheritdoc/>
        public async Task<BaseResult<UserProfileDto>> GetUserProfileAsync(Guid userId)
        {
            var profileDto = await cacheService.GetOrAddToCache(
                $"{CacheKeys.UserProfile}{userId}",
                async () => await mediator.Send(new GetUserProfileDtoByUserIdQuery(userId)));

            if (profileDto is null)
            {
                return BaseResult<UserProfileDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            return BaseResult<UserProfileDto>.Success(profileDto);
        }

        /// <inheritdoc/>
        public async Task<BaseResult> UpdateUserProfileAsync(UpdateUserProfileDto dto, Guid userId)
        {
            var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

            if (profile is null)
            {
                return BaseResult.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            await mediator.Send(new UpdateUserProfileCommand(dto, profile));

            // Создаем транзакцию Redis для того, чтобы гарантировать атомарность операций кэширования,
            // связанных с обновлением профиля пользователя.
            var redisTransaction = redisDatabase.CreateTransaction();

            var userProfileKey = $"{CacheKeys.UserProfile}:{userId}";

            await cacheService.RemoveAsync(userProfileKey);
            await cacheService.SetObjectAsync(userProfileKey, profile);

            bool committed = await redisTransaction.ExecuteAsync();

            if (!committed)
            {
                logger.Error(ErrorMessage.RedisTransactionFailed);

                return BaseResult.Failure((int)ErrorCodes.RedisTransactionFailed, ErrorMessage.RedisTransactionFailed);
            }

            return BaseResult.Success();
        }
    }
}
