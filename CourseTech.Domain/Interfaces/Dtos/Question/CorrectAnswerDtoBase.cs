using CourseTech.Domain.Dto.Question.CorrectAnswer;
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
    public int Id { get; set; }

    /// <summary>
    /// Правильный ответ.
    /// </summary>
    public string CorrectAnswer { get; set; }

    /// <summary>
    /// Оценка пользователя за вопрос
    /// </summary>
    public float UserGrade { get; set; }

    /// <summary>
    /// Правильно ли пользователь ответил на вопрос
    /// </summary>
    public bool AnswerCorrectness { get; set; }
}
