using CourseTech.Domain.Dto.Statistic;

namespace CourseTech.Domain.Interfaces.Services;

public interface IStatisticService
{
    /// <summary>
    /// Получение статистики пользователя.
    /// </summary>
    Task<Result<UserStatisticsDto>> GetUserStatisticsAsync(Guid userId);

    /// <summary>
    /// Получение прогресса по категориям.
    /// </summary>
    Task<Result<CategoryProgressDto[]>> GetCategoryProgressAsync(Guid userId);

    /// <summary>
    /// Получение сводки по сессии.
    /// </summary>
    Task<Result<SessionSummaryDto>> GetSessionSummaryAsync(long sessionId, Guid userId);
}
