using AutoMapper;
using CourseTech.Application.Resources;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Extensions;
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
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .Select(x => mapper.Map<UserProfileDto>(x))
                .FirstOrDefaultAsync();

            if (profileDto is null)
            {
                return BaseResult<UserProfileDto>.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            return BaseResult<UserProfileDto>.Success(profileDto);
        }

        public async Task<BaseResult> UpdateUserProfileAsync(UpdateUserProfileDto dto, Guid userId)
        {
            var profile = await userProfileRepository.GetAll()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (profile is null)
            {
                BaseResult.Failure((int)ErrorCodes.UserProfileNotFound, ErrorMessage.UserProfileNotFound);
            }

            var dateOfBirth = dto.DateOfBirth;

            profile.Name = dto.Name;
            profile.Surname = dto.Surname;
            profile.DateOfBirth = dateOfBirth;
            profile.Age = dateOfBirth.GetYearsByDateToNow();

            userProfileRepository.Update(profile);

            await userProfileRepository.SaveChangesAsync();

            return BaseResult.Success();
        }
    }
}
