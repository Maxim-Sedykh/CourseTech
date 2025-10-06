using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Token;
using CourseTech.Domain.Dto.User;

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
    Task<Result<UserDto>> Register(RegisterUserDto dto);

    /// <summary>
    /// Авторизация пользователя.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result<TokenDto>> Login(LoginUserDto dto);

    /// <summary>
    /// Выйти из аккаунта.
    /// </summary>
    /// <returns></returns>
    Task<Result> LogoutAsync(Guid userId);
}
