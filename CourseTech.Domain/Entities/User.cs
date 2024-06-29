using CourseTech.Domain.Interfaces;

namespace CourseTech.Domain.Entities;

public class User : IEntityId<Guid>, IAuditable
{
    public Guid Id { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public List<Review> Reviews { get; set; }

    public List<LessonRecord> LessonRecords { get; set; }

    public UserProfile UserProfile { get; set; }

    public List<Role> Roles { get; set; }

    public UserToken UserToken { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
