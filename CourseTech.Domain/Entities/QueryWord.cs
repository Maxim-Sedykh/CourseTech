using CourseTech.Domain.Interfaces;

namespace CourseTech.Domain.Entities;

public class QueryWord : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    public int Number { get; set; }

    public int KeywordId { get; set; }

    public Keyword Keyword { get; set; }

    public int QuestionId { get; set; }

    public Question Question { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
