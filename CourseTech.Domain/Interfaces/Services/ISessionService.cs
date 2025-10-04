using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface ISessionService
    {
        /// <summary>
        /// Начало новой сессии.
        /// </summary>
        Task<DataResult<SessionDto>> StartSessionAsync(SessionConfigDto config, Guid userId);

        /// <summary>
        /// Получение следующего вопроса для сессии.
        /// </summary>
        Task<DataResult<QuestionDto>> GetNextQuestionAsync(long sessionId, Guid userId);

        /// <summary>
        /// Завершение сессии.
        /// </summary>
        Task<DataResult<SessionDto>> FinishSessionAsync(long sessionId, Guid userId);

        /// <summary>
        /// Получение сессии по идентификатору.
        /// </summary>
        Task<DataResult<SessionDto>> GetSessionByIdAsync(long sessionId, Guid userId);
    }
}
