using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class Answer : IEntityId<long>, ICreatable
{
    public long Id { get; set; }

    public long SessionId { get; set; }

    public long QuestionId { get; set; }

    public string AudioFileUrl { get; set; }

    public string TranscribedText { get; set; }

    public Session Session { get; set; }

    public Question Question { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
