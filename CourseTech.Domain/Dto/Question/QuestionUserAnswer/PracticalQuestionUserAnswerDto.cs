using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.QuestionUserAnswer;

/// <summary>
/// Модель данных для ответа пользователя на вопрос практического типа.
/// </summary>
public class PracticalQuestionUserAnswerDto : UserAnswerDtoBase
{
    public string UserCodeAnswer { get; set; }
}
