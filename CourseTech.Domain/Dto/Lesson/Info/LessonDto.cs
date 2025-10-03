using CourseTech.Domain.Enum;

namespace CourseTech.Domain.Dto.Lesson.Info;

/// <summary>
/// Модель данных для передачи данных об уроке.
/// Без разметки для лекции.
/// </summary>
public class LessonDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public LessonTypes LessonType { get; set; }
}
