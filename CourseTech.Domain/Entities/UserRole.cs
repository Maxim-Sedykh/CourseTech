namespace CourseTech.Domain.Entities;

/// <summary>
/// Сущность для сценки сущности "Роль" и "Пользователь".
/// Для реализации связи вида "Многие ко многим".
/// </summary>
public class UserRole
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public long RoleId { get; set; }
}
