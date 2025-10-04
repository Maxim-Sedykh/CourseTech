using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface IStatisticService
    {
        /// <summary>
        /// Получение статистики пользователя.
        /// </summary>
        Task<DataResult<UserStatisticsDto>> GetUserStatisticsAsync(Guid userId);

        /// <summary>
        /// Получение прогресса по категориям.
        /// </summary>
        Task<CollectionResult<CategoryProgressDto>> GetCategoryProgressAsync(Guid userId);

        /// <summary>
        /// Получение сводки по сессии.
        /// </summary>
        Task<DataResult<SessionSummaryDto>> GetSessionSummaryAsync(long sessionId, Guid userId);
    }
}
