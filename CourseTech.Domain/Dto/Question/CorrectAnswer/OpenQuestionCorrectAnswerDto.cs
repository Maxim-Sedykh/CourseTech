using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.Pass;

/// <summary>
/// Модель данных для отображения правильного ответа на вопрос открытого типа.
/// </summary>
public class OpenQuestionCorrectAnswerDto : ICorrectAnswerDto
{
    public int Id { get; set; }

    public string CorrectAnswer { get; set; }

    public bool AnswerCorrectness { get; set; }
    public string QuestionType { get; set; } = "OpenQuestionCorrectAnswerDto";
}
