using CourseTech.Domain.Dto.Question.CorrectAnswer;
using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Dto.Question.Pass;
using System.Text.Json.Serialization;

namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Интерфейс для моделей, которые передают правильные ответы.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TestQuestionCorrectAnswerDto), "TestQuestionCorrectAnswerDto")]
[JsonDerivedType(typeof(OpenQuestionCorrectAnswerDto), "OpenQuestionCorrectAnswerDto")]
[JsonDerivedType(typeof(PracticalQuestionCorrectAnswerDto), "PracticalQuestionCorrectAnswerDto")]
public abstract class CorrectAnswerDtoBase
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    public abstract int Id { get; set; }

    /// <summary>
    /// Правильный ответ.
    /// </summary>
    public abstract string CorrectAnswer { get; set; }

    public abstract string QuestionType { get; set; }
}
