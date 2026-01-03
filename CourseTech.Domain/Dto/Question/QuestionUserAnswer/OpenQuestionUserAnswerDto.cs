using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer;

/// <summary>
/// Модель данных для ответа пользователя на вопрос открытого типа.
/// </summary>
public class OpenQuestionUserAnswerDto : UserAnswerDtoBase
{
    public int QuestionId { get; set; }

    public string UserAnswer { get; set; }
    public string QuestionType { get; set; } = "OpenQuestionUserAnswerDto";
}
