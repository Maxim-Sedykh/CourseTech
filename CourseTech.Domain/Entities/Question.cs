using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Вопрос, из практической части урока.
/// Базовая сущность для вопроса. Используется подход TPH (Table-Per-Hierarchy) для базы данных при наследовании сущностей.
/// От неё наследуются сущности для тестового вопроса, открытого вопроса и практического вопроса.
/// </summary>
public class Question : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    /// <summary>
    /// Идентификатор урока.
    /// </summary>
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    /// <summary>
    /// Урок.
    /// </summary>
    public string Title { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
