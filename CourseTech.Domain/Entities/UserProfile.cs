using CourseTech.Domain.Interfaces;

namespace CourseTech.Domain.Entities;

public class UserProfile : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public int Age { get; set; }

    public bool IsExamCompleted { get; set; }

    public float CurrentGrade { get; set; }

    public int LessonsCompleted { get; set; }

    public string Analys { get; set; }

    public bool IsEditAble { get; set; }

    public bool IsReviewLeft { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
