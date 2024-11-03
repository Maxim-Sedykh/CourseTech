using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Пользователь.
/// </summary>
public class User : IEntityId<Guid>, IAuditable
{
    public Guid Id { get; set; }

    /// <summary>
    /// Логин пользователя для входа в аккаунт.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Пароль пользователя для входа в аккаунт.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Отзывы пользователя.
    /// </summary>
    public List<Review> Reviews { get; set; }

    /// <summary>
    /// Записи о прохождение уроков пользователем.
    /// </summary>
    public List<LessonRecord> LessonRecords { get; set; }

    /// <summary>
    /// Профиль пользователя.
    /// </summary>
    public UserProfile UserProfile { get; set; }

    /// <summary>
    /// Роли, которыми обладает пользователь.
    /// </summary>
    public List<Role> Roles { get; set; }

    /// <summary>
    /// Refresh-токен пользователя.
    /// </summary>
    public UserToken UserToken { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
