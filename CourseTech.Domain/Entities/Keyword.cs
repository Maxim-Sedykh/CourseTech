using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

public class Keyword : IEntityId<int>
{
    public int Id { get; set; }

    public string Word { get; set; }

    public List<QueryWord> QueryWords { get; set; }
}
