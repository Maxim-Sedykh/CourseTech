using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Interfaces.Repositories.Base;

/// <summary>
/// Абстракция для взаимодействия с базой данных, через DbContext.
/// Предоставляет основные операции CRUD.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity, TId> : IStateSaveChanges
    where TEntity : class, IEntityId<TId>
    where TId : notnull
{
    /// <summary>
    /// Получить сущность по её идентификатору.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity> GetByIdAsync(TId id);

    /// <summary>
    /// Возвращает все сущности как IQueryable.
    /// </summary>
    /// <returns></returns>
    IQueryable<TEntity> Query();

    /// <summary>
    /// Создает новую сущность.
    /// </summary>
    /// <param name="entity">Сущность для создания.</param>
    /// <returns>Созданная сущность.</returns>
    Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Обновляет сущность.
    /// </summary>
    /// <param name="entity">Сущность для обновления.</param>
    /// <returns>Обновленная сущность.</returns>
    TEntity Update(TEntity entity);

    /// <summary>
    /// Удаляет сущность.
    /// </summary>
    /// <param name="entity">Сущность для удаления.</param>
    void Remove(TEntity entity);
}
