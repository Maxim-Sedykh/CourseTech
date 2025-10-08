using CourseTech.Domain.Dto.Question;

namespace CourseTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис для работы с вопросами после урока, практической частью.
/// </summary>
public interface IQuestionService
{
    /// <summary>
    /// Получение случайного вопроса по критериям.
    /// </summary>
    Task<Result<QuestionDto>> GetRandomQuestionAsync(QuestionFilterDto filter);

    /// <summary>
    /// Получение вопроса по идентификатору.
    /// </summary>
    Task<Result<QuestionDto>> GetQuestionByIdAsync(int questionId);

    /// <summary>
    /// Получение вопросов по категории.
    /// </summary>
    Task<CollectionResult<QuestionDto>> GetQuestionsByCategoryAsync(int categoryId);
}
