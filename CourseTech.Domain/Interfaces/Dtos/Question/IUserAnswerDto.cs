namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Интерфейс для передачи коллекции ответов пользователя на сервер.
/// </summary>
public interface IUserAnswerDto
{
    /// <summary>
    /// Идентификатор вопроса
    /// </summary>
    int QuestionId { get; set; }

    string QuestionType { get; set; }
}
