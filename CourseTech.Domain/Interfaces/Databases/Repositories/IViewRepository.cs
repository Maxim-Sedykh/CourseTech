namespace CourseTech.Domain.Interfaces.Databases.Repositories;

/// <summary>
/// Репозиторий для работы с представлениями (Views)
/// </summary>
/// <typeparam name="TView"></typeparam>
public interface IViewRepository<TView>
{
    /// <summary>
    /// Получить все строки из представления
    /// </summary>
    /// <returns></returns>
    Task<List<TView>> GetAllFromViewAsync();
}
