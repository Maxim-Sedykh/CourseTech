using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис для работы с вопросами после урока, практической частью.
/// </summary>
public interface IQuestionService
{
    Task<Question> GetRandomQuestionAsync(Guid categoryId, string difficulty, List<Guid> excludedQuestionIds);
    Task<Question> GetQuestionByIdAsync(Guid questionId);
    Task<List<Question>> GetQuestionsByCategoryAsync(Guid categoryId);
}
