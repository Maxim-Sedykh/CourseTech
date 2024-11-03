namespace CourseTech.Domain.Interfaces.Entities;

/// <summary>
/// Интерфейс для идентификатора сущности
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IEntityId<T> where T : struct
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public T Id { get; set; }
}
