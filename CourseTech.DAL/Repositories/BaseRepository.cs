using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly CourseDbContext _dbContext;

    public BaseRepository(CourseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>().AsQueryable();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        await _dbContext.AddAsync(entity);

        return entity;
    }

    public void Remove(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        _dbContext.Remove(entity);
    }

    public TEntity Update(TEntity entity)
    {
        ValidateEntityOnNull(entity);

        _dbContext.Update(entity);

        return entity;
    }

    private void ValidateEntityOnNull(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity is null");
        }
    }

    public async Task<TEntity> GetOne<T>(T id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }
}
