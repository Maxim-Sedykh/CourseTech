using CourseTech.Domain.Enum;

namespace CourseTech.Domain.Dto.Lesson
{
    public class LessonPassDto
    {
        public int LessonId { get; set; }

        public LessonType LessonType { get; set; }

        public List<QuestionDto> Questions { get; set; }
    }
}

