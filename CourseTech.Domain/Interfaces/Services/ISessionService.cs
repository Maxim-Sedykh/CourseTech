using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Dto.Session;

namespace CourseTech.Domain.Interfaces.Services;

public interface ISessionService
{
    /// <summary>
    /// Начало новой сессии.
    /// </summary>
    Task<Result<SessionDto>> StartSessionAsync(SessionConfigDto config, Guid userId);

    /// <summary>
    /// Получение следующего вопроса для сессии.
    /// </summary>
    Task<Result<QuestionDto>> GetNextQuestionAsync(long sessionId, Guid userId);

    /// <summary>
    /// Завершение сессии.
    /// </summary>
    Task<Result<SessionDto>> FinishSessionAsync(long sessionId, Guid userId);

    /// <summary>
    /// Получение сессии по идентификатору.
    /// </summary>
    Task<Result<SessionDto>> GetSessionByIdAsync(long sessionId, Guid userId);
}
