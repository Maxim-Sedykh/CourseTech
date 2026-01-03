using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Question.Get;

/// <summary>
/// Модель данных для отображения вопроса открытого типа.
/// </summary>
public class OpenQuestionDto : QuestionDtoBase
{
    public int Id { get; set; }

    public int Number { get; set; }

    public string DisplayQuestion { get; set; }
    public string QuestionType { get; set; } = "OpenQuestionDto";
}
