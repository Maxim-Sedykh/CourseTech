using CourseTech.Domain.Result;
using Microsoft.AspNetCore.Http;

namespace CourseTech.Domain.Interfaces.Services
{
    public interface IAnswerService
    {
        /// <summary>
        /// Обработка ответа пользователя.
        /// </summary>
        Task<DataResult<AnswerResultDto>> ProcessAnswerAsync(ProcessAnswerDto dto, Guid userId);

        /// <summary>
        /// Получение анализа ответа.
        /// </summary>
        Task<DataResult<AnswerAnalysisDto>> GetAnswerAnalysisAsync(long answerId, Guid userId);

        /// <summary>
        /// Получение ответов пользователя.
        /// </summary>
        Task<CollectionResult<AnswerDto>> GetUserAnswersAsync(Guid userId, AnswerFilterDto filter);
    }
}
