using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services;

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
    Task<DataResult<UserDto>> Register(RegisterUserDto dto);

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<DataResult<TokenDto>> Login(LoginUserDto dto);

    /// <summary>
    /// Выйти из аккаунта.
    /// </summary>
    /// <returns></returns>
    Task<BaseResult> LogoutAsync();
}
