using CourseTech.Domain.Interfaces;

namespace CourseTech.Domain.Entities;

public class TestVariant : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    public int QuestionId { get; set; }

    public Question Question { get; set; }

    public int VariantNumber { get; set; }

    public string Content { get; set; }

    public bool IsRight { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
