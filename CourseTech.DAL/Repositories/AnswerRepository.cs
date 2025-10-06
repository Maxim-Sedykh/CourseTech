using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories
{
    public class AnswerRepository(CourseDbContext dbContext) : BaseRepository<Answer, long>(dbContext), IAnswerRepository
    {
        public async Task<List<Answer>> GetBySessionIdAsync(long sessionId)
        {
            return await _table
                .Include(a => a.Question)
                .ThenInclude(q => q.Category)
                .Where(a => a.SessionId == sessionId)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Answer>> GetByUserIdAsync(Guid userId)
        {
            return await _table
                .Include(a => a.Session)
                .Include(a => a.Question)
                .ThenInclude(q => q.Category)
                .Where(a => a.Session.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Answer>> GetByUserIdAsync(Guid userId, int page, int pageSize, int? categoryId = null)
        {
            var query = _table
                .Include(a => a.Session)
                .Include(a => a.Question)
                .ThenInclude(q => q.Category)
                .Where(a => a.Session.UserId == userId);

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.Session.CategoryId == categoryId.Value);
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Answer>> GetByUserIdAndCategoryAsync(Guid userId, int categoryId)
        {
            return await _table
                .Include(a => a.Session)
                .Include(a => a.Question)
                .Where(a => a.Session.UserId == userId && a.Session.CategoryId == categoryId)
                .OrderBy(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<long>> GetQuestionIdsBySessionIdAsync(long sessionId)
        {
            return await _table
                .Where(a => a.SessionId == sessionId)
                .Select(a => a.QuestionId)
                .ToListAsync();
        }
    }
}
