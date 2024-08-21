using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class Review : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }

    public string ReviewText { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
