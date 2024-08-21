namespace CourseTech.Domain.Interfaces.Entities;

public interface IAuditable
{
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
