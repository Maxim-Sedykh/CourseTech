using CourseTech.Application.Commands.UserProfileCommands;
using CourseTech.Domain;
using CourseTech.Domain.Constants.Cache;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Cache;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using MediatR;

namespace CourseTech.Application.Services;

public class UserProfileService(
    ICacheService cacheService,
    IUserProfileRepository userProfileRepository,
    IMediator mediator) : IUserProfileService
{
    /// <inheritdoc/>
    public async Task<Result<UserProfileDto>> GetUserProfileAsync(Guid userId)
    {
        var profileDto = await cacheService.GetOrAddToCache(
            $"{CacheKeys.UserProfile}{userId}",
            async () => await userProfileRepository.GetByUserIdAsync(userId));

        if (profileDto is null)
        {
            return Result<UserProfileDto>.Failure("User profile not found");
        }

        return Result.Success(new UserProfileDto()); //TODO заглушечка впадлу мапить пока
    }

    /// <inheritdoc/>
    public async Task<Result> UpdateUserProfileAsync(UpdateUserProfileDto dto, Guid userId)
    {
        var profile = await userProfileRepository.GetByUserIdAsync(userId);
        if (profile is null)
        {
            return Result.Failure("User profile not found");
        }

        await mediator.Send(new UpdateUserProfileCommand(dto, profile));

        var userProfileKey = $"{CacheKeys.UserProfile}:{userId}";

        await cacheService.RemoveAsync(userProfileKey);

        await cacheService.SetObjectAsync(userProfileKey, profile);

        return Result.Success();
    }
}
