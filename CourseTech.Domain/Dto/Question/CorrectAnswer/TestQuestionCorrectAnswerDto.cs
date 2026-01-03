using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.Pass;

/// <summary>
/// Модель данных для отображения правильного ответа на вопрос тестового типа.
/// </summary>
public class TestQuestionCorrectAnswerDto : CorrectAnswerDtoBase
{
    public int Id { get; set; }

    public string CorrectAnswer { get; set; }

    public bool AnswerCorrectness { get; set; }

    public string QuestionType { get; set; } = "TestQuestionCorrectAnswerDto";
}
