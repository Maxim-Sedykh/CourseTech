using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services.Question;

/// <summary>
/// Сервис для работы с вопросами после урока, практической частью.
/// </summary>
public interface IQuestionService
{
    /// <summary>
    /// Получение вопросов для тестирования по идентификатору урока.
    /// </summary>
    /// <param name="lessonId"></param>
    /// <returns></returns>
    Task<DataResult<LessonPracticeDto>> GetLessonQuestionsAsync(int lessonId);

    /// <summary>
    /// Завершение прохождения тестирования.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<DataResult<PracticeCorrectAnswersDto>> PassLessonQuestionsAsync(PracticeUserAnswersDto dto, Guid userId);
}
