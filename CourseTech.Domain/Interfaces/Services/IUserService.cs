using CourseTech.Domain.Dto.User;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с пользователями
    /// ( Недоступен для обычных пользователей ).
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<UserDto>> GetUsersAsync();

        /// <summary>
        /// Удаление пользователя по идентификатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteUserAsync(Guid userId);

        /// <summary>
        /// Получение данных пользователя по идентификатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<UpdateUserDto>> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Обновление данных пользователя.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<UpdateUserDto>> UpdateUserDataAsync(UpdateUserDto dto);
    }
}
