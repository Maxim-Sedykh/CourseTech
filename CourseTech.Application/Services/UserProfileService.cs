using AutoMapper;
using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Application.Queries.UserProfileQueries;
using CourseTech.Application.Queries.UserQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Extensions;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CourseTech.Application.Services
{
    public class UserProfileService(ICacheService cacheService, IDatabase redisDatabase, IMediator mediator) : IUserProfileService
    {
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
                return BaseResult.Failure((int)ErrorCodes.RedisTransactionFailed, ErrorMessage.RedisTransactionFailed);
            }

            return BaseResult.Success();
        }
    }
}
