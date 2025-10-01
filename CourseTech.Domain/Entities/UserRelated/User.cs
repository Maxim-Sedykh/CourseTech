using CourseTech.Domain.Enum;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities.UserRelated;

/// <summary>
/// Пользователь.
/// </summary>
public class User : IEntityId<Guid>, IAuditable
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Email пользователя.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Логин пользователя для входа в аккаунт.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Пароль пользователя для входа в аккаунт.
    /// </summary>
    public string Password { get; set; }

    public Role Role { get; set; }

    public int SubscriptionId { get; set; }

    public Subscription Subscription { get; set; }

    /// <summary>
    /// Профиль пользователя.
    /// </summary>
    public UserProfile UserProfile { get; set; }

    /// <summary>
    /// Refresh-токен пользователя.
    /// </summary>
    public UserToken UserToken { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
