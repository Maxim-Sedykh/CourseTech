using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories.Base;

namespace CourseTech.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
        Task<User> GetByLoginAsync(string login);
        Task<User> GetByEmailAsync(string email);
    }
}
