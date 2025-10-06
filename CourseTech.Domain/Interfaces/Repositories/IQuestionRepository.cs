using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories.Base;

namespace CourseTech.Domain.Interfaces.Repositories
{
    public interface IQuestionRepository : IBaseRepository<Question, int>
    {
        Task<Question> GetRandomAsync(int categoryId, string difficulty, List<int> excludedQuestionIds);
        Task<List<Question>> GetByCategoryIdAsync(int categoryId);
        Task<int> GetCountByCategoryIdAsync(int categoryId);
    }
}
