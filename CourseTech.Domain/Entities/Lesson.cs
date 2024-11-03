using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Урок.
/// </summary>
public class Lesson : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    /// <summary>
    /// Тема урока.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Тип урока.
    /// </summary>
    public LessonTypes LessonType { get; set; }

    /// <summary>
    /// HTML-разметка для лекционной части урока.
    /// </summary>
    public string LectureMarkup { get; set; }

    /// <summary>
    /// Вопросы практической части урока.
    /// </summary>
    public List<Question> Questions { get; set; }

    /// <summary>
    /// Записи прохождений пользователями урока.
    /// </summary>
    public List<LessonRecord> LessonRecords { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
