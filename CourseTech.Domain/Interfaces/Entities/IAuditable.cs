namespace CourseTech.Domain.Interfaces.Entities;

/// <summary>
/// Интерфейс для аудита различных сущностей.
/// Содержит информацию о времени создания и последнего обновления.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Получает или задает дату и время создания сущности.
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Получает или задает дату и время последнего обновления сущности.
    /// Значение может быть null, если сущность еще не была обновлена.
    /// </summary>
    DateTime? UpdatedAt { get; set; }
}
