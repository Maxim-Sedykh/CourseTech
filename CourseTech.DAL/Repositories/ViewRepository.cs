using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories
{
    public class ViewRepository<TView> : IViewRepository<TView> where TView : class
    {
        private readonly CourseDbContext _dbContext;
        private readonly DbSet<TView> _view = null;

        public ViewRepository(CourseDbContext dbContext)
        {
            _dbContext = dbContext;
            _view = _dbContext.Set<TView>();
        }

        /// <inheritdoc/>
        public async Task<List<TView>> GetAllFromViewAsync() => await _view.ToListAsync();
    }
}
