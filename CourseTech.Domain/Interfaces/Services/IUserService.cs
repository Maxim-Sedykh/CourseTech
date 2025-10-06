using CourseTech.Domain.Dto.User;

namespace CourseTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис для работы с пользователями
/// ( Функционал для админа ).
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Получение всех пользователей.
    /// </summary>
    /// <returns></returns>
    Task<Result<UserDto>> GetUsersAsync();

    /// <summary>
    /// Удаление пользователя по идентификатору.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result> DeleteUserAsync(Guid userId);

    /// <summary>
    /// Получение данных пользователя по идентификатору.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result<UpdateUserDto>> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Обновление данных пользователя.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<Result<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto);
}
