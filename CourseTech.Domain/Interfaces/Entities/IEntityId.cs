namespace CourseTech.Domain.Interfaces.Entities;

/// <summary>
/// Интерфейс для идентификатора сущности
/// </summary>
/// <typeparam name="T">Тип сущности</typeparam>
public interface IEntityId<T>
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    T Id { get; set; }
}
