using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Отзыв пользователя.
/// </summary>
public class Review : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя, сделавшего отзыв.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь, который создал отзыв.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Текст отзыва.
    /// </summary>
    public string ReviewText { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
