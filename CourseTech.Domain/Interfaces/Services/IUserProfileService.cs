using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с профилями пользователя
    /// </summary>
    public interface IUserProfileService
    {
        /// <summary>
        /// Получение профиля пользователя по его идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<UserProfileDto>> GetUserProfileAsync(Guid userId);

        /// <summary>
        /// Обновление профиля пользователя
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateUserProfileAsync(UserProfileDto dto);
    }
}
