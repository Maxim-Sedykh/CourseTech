using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Lesson.Practice;

/// <summary>
/// Модель данных для отправки ответов пользователя на практическую часть на сервер.
/// </summary>
public class PracticeUserAnswersDto
{
    public int LessonId { get; set; }

    public bool IsDemoMode { get; set; }

    public List<IUserAnswerDto> UserAnswerDtos { get; set; }
}
