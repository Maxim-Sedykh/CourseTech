using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories
{
    public class UserTokenRepository(CourseDbContext dbContext) : BaseRepository<UserToken, long>(dbContext), IUserTokenRepository
    {
        public async Task<UserToken> GetByUserIdAsync(Guid userId)
        {
            return await _table
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.UserId == userId);
        }

        public async Task<UserToken> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _table
                .Include(ut => ut.User)
                .FirstOrDefaultAsync(ut => ut.RefreshToken == refreshToken);
        }
    }
}
