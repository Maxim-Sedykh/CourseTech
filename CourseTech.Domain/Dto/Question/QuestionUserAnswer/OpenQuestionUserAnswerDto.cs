using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer;

/// <summary>
/// Модель данных для ответа пользователя на вопрос открытого типа.
/// </summary>
public class OpenQuestionUserAnswerDto : UserAnswerDtoBase
{
    public string UserAnswer { get; set; }
}
