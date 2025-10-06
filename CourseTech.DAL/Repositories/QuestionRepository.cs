using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories
{
    public class QuestionRepository(CourseDbContext dbContext) : BaseRepository<Question, int>(dbContext), IQuestionRepository
    {
        public async Task<Question> GetRandomAsync(int categoryId, string difficulty, List<int> excludedQuestionIds)
        {
            var query = _table
                .Include(q => q.Category)
                .Where(q => q.CategoryId == categoryId);

            if (!string.IsNullOrEmpty(difficulty))
            {
                query = query.Where(q => q.Difficulty == difficulty);
            }

            if (excludedQuestionIds != null && excludedQuestionIds.Any())
            {
                query = query.Where(q => !excludedQuestionIds.Contains(q.Id));
            }

            return await query
                .OrderBy(q => EF.Functions.Random())
                .FirstOrDefaultAsync();
        }

        public async Task<List<Question>> GetByCategoryIdAsync(int categoryId)
        {
            return await _table
                .Include(q => q.Category)
                .Where(q => q.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<int> GetCountByCategoryIdAsync(int categoryId)
        {
            return await _table
                .CountAsync(q => q.CategoryId == categoryId);
        }
    }
}
