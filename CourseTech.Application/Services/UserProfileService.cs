using CourseTech.Domain;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Services;
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
    public async Task<Result> UpdateUserProfileAsync(UpdateUserProfileDto dto, Guid userId)
    {
        var profile = await mediator.Send(new GetProfileByUserIdQuery(userId));

        if (profile is null)
        {
            return Result.Failure((int)ErrorCode.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
        }

        await mediator.Send(new UpdateUserProfileCommand(dto, profile));

        var userProfileKey = $"{CacheKeys.UserProfile}:{userId}";

        await cacheService.RemoveAsync(userProfileKey);

        await cacheService.SetObjectAsync(userProfileKey, profile);

        return Result.Success();
    }
}
