namespace CourseTech.Domain.Interfaces.Databases;

/// <summary>
/// Интерфейс для реализации сохранения состояния сущностей.
/// </summary>
public interface IStateSaveChanges
{
    /// <summary>
    /// Сохранить все изменения сущностей.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
