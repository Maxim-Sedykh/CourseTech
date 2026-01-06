using CourseTech.Domain.Dto.Question.CheckQuestions;
using System.Text.Json.Serialization;

namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Интерфейс для модели, в которой указаны данные для проверки правильности вопроса.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(OpenQuestionCheckingDto), "OpenQuestionCheckingDto")]
[JsonDerivedType(typeof(TestQuestionCheckingDto), "TestQuestionCheckingDto")]
[JsonDerivedType(typeof(PracticalQuestionCheckingDto), "PracticalQuestionCheckingDto")]
public abstract class CheckQuestionDtoBase
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    public int QuestionId { get; set; }
}
