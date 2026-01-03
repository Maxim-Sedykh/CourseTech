using CourseTech.Domain.Dto.Question.CheckQuestions;
using CourseTech.Domain.Dto.Question.Get;
using System.Text.Json.Serialization;

namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Интерфейс для модели, в которой указаны данные для проверки правильности вопроса.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(OpenQuestionCheckingDto), "TestQuestionDto")]
[JsonDerivedType(typeof(TestQuestionCheckingDto), "OpenQuestionDto")]
[JsonDerivedType(typeof(PracticalQuestionCheckingDto), "PracticalQuestionDto")]
public abstract class CheckQuestionDtoBase
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    public abstract int QuestionId { get; set; }
}
