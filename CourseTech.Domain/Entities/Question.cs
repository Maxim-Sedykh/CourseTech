using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class Question : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public Lesson Lesson { get; set; }

    public int Number { get; set; }

    public QuestionType Type { get; set; }

    public string DisplayQuestion { get; set; }

    public string Answer { get; set; }

    public List<TestVariant> TestVariants { get; set; }

    public List<QueryWord> QueryWords { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
