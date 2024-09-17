using CourseTech.Domain.Entities.QuestionEntities.QuestionTypesEntities;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class TestVariant : IEntityId<int>, IAuditable
{
    public int Id { get; set; }

    public int TestQuestionId { get; set; }

    public TestQuestion TestQuestion { get; set; }

    public int VariantNumber { get; set; }

    public string Content { get; set; }

    public bool IsRight { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
