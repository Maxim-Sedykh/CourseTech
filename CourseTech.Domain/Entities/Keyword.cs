using CourseTech.Domain.Interfaces;

namespace CourseTech.Domain.Entities;

public class Keyword : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    public string Word { get; set; }

    public List<QueryWord> QueryWords { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
