using CourseTech.Domain.Dto.Lesson.Info;

namespace CourseTech.Domain.Dto.Lesson;

/// <summary>
/// Модель данных для отображения уроков для пользователя.
/// передаётся количество пройденных уроков пользователем. 
/// Для того чтобы блокировать доступ пользователю к урокам, до которых он ещё не дошёл.
/// </summary>
public class UserLessonsDto
{
    public int LessonsCompleted { get; set; }

    public List<LessonDto> LessonNames { get; set; }
}
