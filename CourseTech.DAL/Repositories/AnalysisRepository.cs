using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories;

public class AnalysisRepository(CourseDbContext dbContext) : BaseRepository<Analysis, long>(dbContext), IAnalysisRepository
{
    public async Task<Analysis> GetByAnswerIdAsync(long answerId)
    {
        return await _table
            .Include(a => a.Answer)
            .FirstOrDefaultAsync(a => a.AnswerId == answerId);
    }

    public async Task<List<Analysis>> GetByAnswerIdsAsync(List<long> answerIds)
    {
        return await _table
            .Include(a => a.Answer)
            .Where(a => answerIds.Contains(a.AnswerId))
            .ToListAsync();
    }
}
