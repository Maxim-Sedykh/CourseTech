using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Services
{
    public class UserService : IUserService
    {
        public Task<BaseResult> CreateUserAsync(CreateUserDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult> DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UpdateUserDto>> GetUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<CollectionResult<UserDto>> GetUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
