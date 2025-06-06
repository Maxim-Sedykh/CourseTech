﻿namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Интерфейс для моделей, которые передают правильные ответы.
/// </summary>
public interface ICorrectAnswerDto
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Правильный ответ.
    /// </summary>
    string CorrectAnswer { get; set; }

    /// <summary>
    /// Правильный ли ответ у пользователя?
    /// </summary>
    bool AnswerCorrectness { get; set; }

    string QuestionType { get; set; }
}
