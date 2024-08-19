using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Result;
using System.Security.Claims;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис предназначенный для авторизации/регистрации
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult> Register(RegisterUserDto dto);

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult> Login(LoginUserDto dto);

        /// <summary>
        /// Смена пароля пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult> ChangePassword(ChangePasswordDto dto, Guid userId);
    }
}
