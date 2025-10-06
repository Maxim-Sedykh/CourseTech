using CourseTech.Domain.Interfaces.Entities;
using CourseTech.Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories.Base;

public class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> 
    where TEntity : class, IEntityId<TId>
    where TId : struct
{
    private readonly CourseDbContext _dbContext;
    protected readonly DbSet<TEntity> _table;

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

    /// <inheritdoc/>
    public async Task<TEntity> GetByIdAsync(TId id)
    {
        var entity = await _table.FindAsync(id);

        return entity;
    }

    private void ValidateEntityOnNull(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity is null");
        }
    }
}
