using CourseTech.Domain.Entities.QuestionEntities;
using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class Lesson : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    public string Name { get; set; }

    public LessonType LessonType { get; set; }

    public string LectureMarkup { get; set; }

    public List<Question> Questions { get; set; }

    public List<LessonRecord> LessonRecords { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
