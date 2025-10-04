using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис для работы с вопросами после урока, практической частью.
/// </summary>
public interface IQuestionService
{
    /// <summary>
    /// Получение случайного вопроса по критериям.
    /// </summary>
    Task<DataResult<QuestionDto>> GetRandomQuestionAsync(QuestionFilterDto filter);

    /// <summary>
    /// Получение вопроса по идентификатору.
    /// </summary>
    Task<DataResult<QuestionDto>> GetQuestionByIdAsync(int questionId);

    /// <summary>
    /// Получение вопросов по категории.
    /// </summary>
    Task<CollectionResult<QuestionDto>> GetQuestionsByCategoryAsync(int categoryId);
}
