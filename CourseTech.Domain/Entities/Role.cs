using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class Role : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    public string Name { get; set; }

    public List<User> Users { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
