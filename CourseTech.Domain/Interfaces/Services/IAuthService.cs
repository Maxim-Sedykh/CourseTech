using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Result;
using System.Security.Claims;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис предназначенный для авторизации/регистрации.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UserDto>> Register(RegisterUserDto dto);

        /// <summary>
        /// Авторизация пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<TokenDto>> Login(LoginUserDto dto);
    }
}
