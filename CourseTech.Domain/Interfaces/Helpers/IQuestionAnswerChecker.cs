using CourseTech.DAL.Views;
using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Interfaces.Helpers;

/// <summary>
/// Сервис для проверки пользовательский ответов по практической части уроков.
/// </summary>
public interface IQuestionAnswerChecker
{
    /// <summary>
    /// Проверка пользовательских ответов.
    /// </summary>
    /// <param name="checkQuestions">Список моделей для проверки ответа пользователя.</param>
    /// <param name="userAnswers">Ответы пользователя.</param>
    /// <param name="userGrade">Итоговая оценка пользователя за пройденные вопросы.</param>
    /// <returns></returns>
    Task<List<CorrectAnswerDtoBase>> CheckUserAnswers(List<CheckQuestionDtoBase> checkQuestions, List<UserAnswerDtoBase> userAnswers, UserGradeDto userGrade, List<QuestionTypeGrade> questionTypeGrades);
}
