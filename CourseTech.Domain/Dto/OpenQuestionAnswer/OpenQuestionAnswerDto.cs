namespace CourseTech.Domain.Dto.OpenQuestionAnswer;

/// <summary>
/// Модель данных для отображения правильного ответа на вопрос открытого типа.
/// </summary>
public class OpenQuestionAnswerDto
{
    public string OpenQuestionCorrectAnswer { get; set; }

    public int QuestionId { get; set; }
}
