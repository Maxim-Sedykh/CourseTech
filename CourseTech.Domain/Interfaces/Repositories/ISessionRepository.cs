using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories.Base;

namespace CourseTech.Domain.Interfaces.Repositories;

public interface ISessionRepository : IBaseRepository<Session, long>
{
    Task<List<Session>> GetByUserIdAsync(Guid userId);
    Task<List<Session>> GetByCategoryIdAsync(int categoryId);
}
