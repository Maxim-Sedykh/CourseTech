﻿using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities.QuestionEntities;

/// <summary>
/// Вопрос, из практической части урока.
/// Базовая сущность для вопроса. Используется подход TPH (Table-Per-Hierarchy) для базы данных при наследовании сущностей.
/// От неё наследуются сущности для тестового вопроса, открытого вопроса и практического вопроса.
/// </summary>
public abstract class BaseQuestion : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    /// <summary>
    /// Номер вопроса.
    /// </summary>
    public int Number { get; set; }

    /// <summary>
    /// Идентификатор урока.
    /// </summary>
    public int LessonId { get; set; }

    /// <summary>
    /// Урок.
    /// </summary>
    public Lesson Lesson { get; set; }

    /// <summary>
    /// Вопрос, который отображается пользователю.
    /// </summary>
    public string DisplayQuestion { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
