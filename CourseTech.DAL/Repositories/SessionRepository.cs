using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories
{
    public class SessionRepository(CourseDbContext dbContext) : BaseRepository<Session, long>(dbContext), ISessionRepository
    {
        public async Task<List<Session>> GetByUserIdAsync(Guid userId)
        {
            return await _table
                .Include(s => s.Category)
                .Include(s => s.Answers)
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Session>> GetByCategoryIdAsync(int categoryId)
        {
            return await _table
                .Include(s => s.User)
                .Include(s => s.Answers)
                .Where(s => s.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
