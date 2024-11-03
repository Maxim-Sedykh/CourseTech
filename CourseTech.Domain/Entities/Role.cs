using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Роль.
/// </summary>
public class Role : IEntityId<long>, IAuditable
{
    public long Id { get; set; }
    
    /// <summary>
    /// Название роли.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Пользователи, у которых есть эта роль.
    /// </summary>
    public List<User> Users { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
