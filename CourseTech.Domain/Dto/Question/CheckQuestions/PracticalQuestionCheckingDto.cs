using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.CheckQuestions;

/// <summary>
/// Модель данных для хранения данных, которые нужны для проверки правильности вопроса практического типа.
/// </summary>
public class PracticalQuestionCheckingDto : ICheckQuestionDto
{
    public int QuestionId { get; set; }

    public string CorrectQueryCode { get; set; }
}
