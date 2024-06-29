using CourseTech.Domain.Interfaces;

namespace CourseTech.Domain.Entities;

public class LessonRecord : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    public int LessonId { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public Lesson Lesson { get; set; }

    public float Mark { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
