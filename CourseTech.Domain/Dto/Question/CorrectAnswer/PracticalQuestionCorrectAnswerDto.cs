using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.Pass;

/// <summary>
/// Модель данных для отображения правильного ответа на вопрос практического типа.
/// </summary>
public class PracticalQuestionCorrectAnswerDto : ICorrectAnswerDto
{
    public int Id { get; set; }

    public string CorrectAnswer { get; set; }

    public float QuestionUserGrade { get; set; }

    public List<dynamic> QueryResult { get; set; }

    public string UserQueryAnalys { get; set; }

    public bool AnswerCorrectness { get; set; }

    public string QuestionType { get; set; } = "PracticalQuestionCorrectAnswerDto";
}
