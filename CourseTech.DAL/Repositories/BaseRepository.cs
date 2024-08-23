using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly CourseDbContext _dbContext;
    private readonly DbSet<TEntity> _table = null;

    public BaseRepository(CourseDbContext dbContext)
    {
        _dbContext = dbContext;
        _table = _dbContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _table.AsQueryable();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        await _table.AddAsync(entity);

        return entity;
    }

    public void Remove(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        _table.Remove(entity);
    }

    public TEntity Update(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        _table.Update(entity);

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
