using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CourseTech.Application.Services
{
    public class UserProfileService : IUserProfileService
    {
        //Primary Constructor
        public Task<BaseResult<UserProfileDto>> GetUserProfileAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UserProfileDto>> UpdateUserProfileAsync(UserProfileDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
