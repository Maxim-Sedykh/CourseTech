using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Lesson.Practice;

/// <summary>
/// Модель данных для отображения в практической части правильных ответов пользователю.
/// </summary>
public class PracticeCorrectAnswersDto
{
    public int LessonId { get; set; }

    public List<ICorrectAnswerDto> QuestionCorrectAnswers { get; set; }
}

