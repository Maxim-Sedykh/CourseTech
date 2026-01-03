using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using System.Text.Json.Serialization;

namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Интерфейс для передачи коллекции ответов пользователя на сервер.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(TestQuestionUserAnswerDto), "TestQuestionUserAnswerDto")]
[JsonDerivedType(typeof(OpenQuestionUserAnswerDto), "OpenQuestionUserAnswerDto")]
[JsonDerivedType(typeof(PracticalQuestionUserAnswerDto), "PracticalQuestionUserAnswerDto")]
public abstract class UserAnswerDtoBase
{
    /// <summary>
    /// Идентификатор вопроса
    /// </summary>
    public abstract int QuestionId { get; set; }

    public abstract string QuestionType { get; set; }
}
