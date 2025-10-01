using CourseTech.Application.CQRS.Commands.UserProfileCommands;
using CourseTech.Application.CQRS.Queries.Dtos.UserProfileDtoQuery;
using CourseTech.Application.CQRS.Queries.Entities.UserProfileQueries;
using CourseTech.Application.Resources;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using MediatR;

namespace CourseTech.Application.Services;

public class UserProfileService(
    ICacheService cacheService,
    IMediator mediator) : IUserProfileService
{
    /// <inheritdoc/>
    public async Task<DataResult<UserProfileDto>> GetUserProfileAsync(Guid userId)
    {
        var profileDto = await cacheService.GetOrAddToCache(
            $"{CacheKeys.UserProfile}{userId}",
            async () => await mediator.Send(new GetUserProfileDtoByUserIdQuery(userId)));

        if (profileDto is null)
        {
            return DataResult<UserProfileDto>.Failure((int)ErrorCode.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        return DataResult<UserProfileDto>.Success(profileDto);
    }

    /// <inheritdoc/>
    public async Task<BaseResult> UpdateUserProfileAsync(UpdateUserProfileDto dto, Guid userId)
    {
        var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

        if (profile is null)
        {
            return BaseResult.Failure((int)ErrorCode.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        await mediator.Send(new UpdateUserProfileCommand(dto, profile));

        var userProfileKey = $"{CacheKeys.UserProfile}:{userId}";

        await cacheService.RemoveAsync(userProfileKey);

        await cacheService.SetObjectAsync(userProfileKey, profile);

        return BaseResult.Success();
    }
}
