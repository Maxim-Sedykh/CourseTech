using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Repositories;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.Application.Services
{
    public class UserProfileService(IBaseRepository<UserProfile> userProfileRepository, IMapper mapper) : IUserProfileService
    {
        public async Task<BaseResult<UserProfileDto>> GetUserProfileAsync(Guid userId)
        {
            var profileDto = await userProfileRepository.GetAll()
                        .Select(x => mapper.Map<UserProfileDto>(x))
                        .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profileDto is null)
            {
                return BaseResult<UserProfileDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            return BaseResult<UserProfileDto>.Success(profileDto);
        }

        public async Task<BaseResult> UpdateUserProfileAsync(UserProfileDto dto)
        {
            var profile = await userProfileRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (profile is null)
            {
                BaseResult.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            userProfileRepository.Update(profile);
            await userProfileRepository.SaveChangesAsync();

            return BaseResult.Success();
        }
    }
}
