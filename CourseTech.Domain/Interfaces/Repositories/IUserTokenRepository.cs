using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories.Base;

namespace CourseTech.Domain.Interfaces.Repositories
{

    public interface IUserTokenRepository : IBaseRepository<UserToken, long>;
}
