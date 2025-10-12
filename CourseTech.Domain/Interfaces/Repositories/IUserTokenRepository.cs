using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories.Base;

namespace CourseTech.Domain.Interfaces.Repositories;


public interface IUserTokenRepository : IBaseRepository<UserToken, long>
{
    Task<UserToken> GetByUserIdAsync(Guid userId);
    Task<UserToken> GetByRefreshTokenAsync(string refreshToken);
}
