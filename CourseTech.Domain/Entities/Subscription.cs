using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class Subscription : IEntityId<int>, IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MaxQuestionsPerDay { get; set; }

    public List<User> Users { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
