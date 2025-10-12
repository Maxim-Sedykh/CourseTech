using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories;

public class UserProfileRepository(CourseDbContext dbContext) : BaseRepository<UserProfile, long>(dbContext), IUserProfileRepository
{
    public async Task<UserProfile> GetByUserIdAsync(Guid userId)
    {
        return await _table
            .Include(up => up.User)
            .FirstOrDefaultAsync(up => up.UserId == userId);
    }
}
