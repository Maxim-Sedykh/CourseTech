using CourseTech.Domain.Dto.Analysis;
using CourseTech.Domain.Dto.Answer;

namespace CourseTech.Domain.Interfaces.Services;

public interface IAnswerService
{
    /// <summary>
    /// Обработка ответа пользователя.
    /// </summary>
    Task<Result<AnswerResultDto>> ProcessAnswerAsync(ProcessAnswerDto dto, Guid userId);

    /// <summary>
    /// Получение анализа ответа.
    /// </summary>
    Task<Result<AnswerAnalysisDto>> GetAnswerAnalysisAsync(long answerId, Guid userId);

    /// <summary>
    /// Получение ответов пользователя.
    /// </summary>
    Task<Result<List<AnswerDto>>> GetUserAnswersAsync(Guid userId, AnswerFilterDto filter);
}
