using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с профилями пользователя.
    /// </summary>
    public interface IUserProfileService
    {
        /// <summary>
        /// Получение профиля пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DataResult<UserProfileDto>> GetUserProfileAsync(Guid userId);

        /// <summary>
        /// Обновление профиля пользовател.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult> UpdateUserProfileAsync(UpdateUserProfileDto dto, Guid userId);
    }
}
