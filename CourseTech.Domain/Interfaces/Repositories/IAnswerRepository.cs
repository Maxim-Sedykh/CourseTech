using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories.Base;

namespace CourseTech.Domain.Interfaces.Repositories
{
    public interface IAnswerRepository : IBaseRepository<Answer, long>
    {
        Task<List<Answer>> GetBySessionIdAsync(long sessionId);
        Task<List<Answer>> GetByUserIdAsync(Guid userId);
        Task<List<Answer>> GetByUserIdAsync(Guid userId, int page, int pageSize, int? categoryId = null);
        Task<List<Answer>> GetByUserIdAndCategoryAsync(Guid userId, int categoryId);
        Task<List<long>> GetQuestionIdsBySessionIdAsync(long sessionId);
    }
}
