using CourseTech.Domain.Dto.Question.GetQuestions;
using System.Text.Json.Serialization;

namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Модель для отображения пользователю списка вопросов в практической части.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TestQuestionDto), "TestQuestionDto")]
[JsonDerivedType(typeof(OpenQuestionDto), "OpenQuestionDto")]
[JsonDerivedType(typeof(PracticalQuestionDto), "PracticalQuestionDto")]
public abstract class QuestionDtoBase
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер вопроса.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Вопрос, который отображается пользователю.
    /// </summary>
    public string DisplayQuestion { get; set; }
}
