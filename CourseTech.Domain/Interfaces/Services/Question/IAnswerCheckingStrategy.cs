using CourseTech.Domain.Dto.Question;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Interfaces.Services.Question;

/// <summary>
/// Интерфейс стратегии для проверки определённого вопроса
/// </summary>
public interface IAnswerCheckingStrategy
{
    /// <summary>
    /// Тип вопроса
    /// </summary>
    Type UserAnswerType { get; }

    /// <summary>
    /// Проверить вопрос на правильность
    /// </summary>
    /// <param name="userAnswer">Ответ пользователя</param>
    /// <param name="checkQuestion">Данные вопроса для его проверки</param>
    /// <param name="questionGrade">Максимальная оценка за вопрос</param>
    /// <returns><see cref="CorrectAnswerDtoBase"/> Модель с данными о результате проверки вопроса</returns>
    Task<CorrectAnswerDtoBase> CheckAnswerAsync(UserAnswerDtoBase userAnswer, CheckQuestionDtoBase checkQuestion, float questionGrade);
}
