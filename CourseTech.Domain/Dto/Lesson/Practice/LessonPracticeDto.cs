using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Lesson.Practice;

/// <summary>
/// Модель данных для отображения практической части урока пользователю.
/// </summary>
public class LessonPracticeDto
{
    public int LessonId { get; set; }

    public LessonTypes LessonType { get; set; }

    public List<QuestionDtoBase> Questions { get; set; }
}
