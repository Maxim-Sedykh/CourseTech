namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Интерфейс для модели, в которой указаны данные для проверки правильности вопроса.
/// </summary>
public interface ICheckQuestionDto
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    int QuestionId { get; set; }
}
