using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities.UserRelated;

/// <summary>
/// Профиль пользователя, для хранения различной информации о пользователе.
/// И его состояния прохождения курса.
/// </summary>
public class UserProfile : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string AvatarUrl { get; set; }

    /// <summary>
    /// Возраст пользователя.
    /// Вычисляется автоматически исходя из даты рождения.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Дата рождения пользователя.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
