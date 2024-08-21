namespace CourseTech.Domain.Interfaces.Entities;

public interface IEntityId<T> where T : struct
{
    public T Id { get; set; }
}
