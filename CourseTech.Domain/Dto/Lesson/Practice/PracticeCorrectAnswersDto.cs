using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Dtos.Question;

namespace CourseTech.Domain.Dto.Lesson.Test
{
    public class PracticeCorrectAnswersDto
    {
        public int LessonId { get; set; }

        public List<ICorrectAnswerDto> QuestionCorrectAnswers { get; set; }
    }
}

