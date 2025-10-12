namespace CourseTech.Domain.Interfaces.Entities;

public interface ICreatable
{
    /// <summary>
    /// Получает или задает дату и время создания сущности.
    /// </summary>
    DateTime CreatedAt { get; set; }
}
