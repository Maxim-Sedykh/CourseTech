using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Запись о прохождении урока пользователем.
/// </summary>
public class LessonRecord : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор урока.
    /// </summary>
    public int LessonId { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Урок.
    /// </summary>
    public Lesson Lesson { get; set; }

    /// <summary>
    /// Оценка, которую пользователь получил за данный урок.
    /// </summary>
    public float Mark { get; set; }

    /// <summary>
    /// Запись прохождения урока пользователя во время демо-режима
    /// Демо-режим - вопросы тестового и открытого типа до лекционного материала
    /// </summary>
    public bool IsDemo { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
