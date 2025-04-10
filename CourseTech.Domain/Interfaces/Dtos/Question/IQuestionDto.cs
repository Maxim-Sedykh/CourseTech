﻿namespace CourseTech.Domain.Interfaces.Dtos.Question;

/// <summary>
/// Модель для отображения пользователю списка вопросов в практической части.
/// </summary>
public interface IQuestionDto
{
    /// <summary>
    /// Идентификатор вопроса.
    /// </summary>
    int Id { get; set; }

    /// <summary>
    /// Номер вопроса.
    /// </summary>
    int Number { get; set; }

    /// <summary>
    /// Вопрос, который отображается пользователю.
    /// </summary>
    string DisplayQuestion { get; set; }

    string QuestionType { get; set; }
}
