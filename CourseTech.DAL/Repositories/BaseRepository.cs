using CourseTech.Domain.Interfaces.Databases.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories;

/// <summary>
/// Generic-репозиторий. Абстракция над DbContext, основные CRUD операции
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly CourseDbContext _dbContext;
    private readonly DbSet<TEntity> _table;

    public BaseRepository(CourseDbContext dbContext)
    {
        _dbContext = dbContext;
        _table = _dbContext.Set<TEntity>();
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> GetAll()
    {
        return _table.AsQueryable();
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        await _table.AddAsync(entity);

        return entity;
    }

    /// <inheritdoc/>
    public void Remove(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        _table.Remove(entity);
    }

    /// <inheritdoc/>
    public TEntity Update(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        _table.Update(entity);

        return entity;
    }
    
    /// <summary>
    /// Валидация сущности на NULL
    /// </summary>
    /// <param name="entity"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private void ValidateEntityOnNull(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity is null");
        }
    }
}
