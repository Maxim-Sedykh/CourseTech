using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.Get;

/// <summary>
/// Модель данных для отображения вопроса практического типа.
/// </summary>
public class PracticalQuestionDto : QuestionDtoBase
{
    public int Id { get; set; }

    public int Number { get; set; }

    public string DisplayQuestion { get; set; }

    public string QuestionType { get; set; } = "PracticalQuestionDto";
}
