using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Interfaces.Services;
using CourseTech.Domain.Result;

namespace CourseTech.Application.Services
{
    public class AuthService : IAuthService
    {
        //Primary Constructor

        public Task<BaseResult> ChangePassword(ChangePasswordDto dto, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult> Login(LoginUserDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResult> Register(RegisterUserDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
