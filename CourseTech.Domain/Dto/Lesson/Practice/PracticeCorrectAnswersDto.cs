using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Lesson.Test;

/// <summary>
/// Модель данных для отображения в практической части правильных ответов пользователю.
/// </summary>
public class PracticeCorrectAnswersDto
{
    public int LessonId { get; set; }

    public List<CorrectAnswerDtoBase> QuestionCorrectAnswers { get; set; }
}

