using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Запись о прохождении урока пользователем.
/// </summary>
public class Session : IEntityId<long>, ICreatable
{
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    public int CategoryId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; }

    public Category Category { get; set; }

    public List<Answer> Answers { get; set; }

    public DateTime? FinishedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}
