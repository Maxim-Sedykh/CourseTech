using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories;

public class UserRepository(CourseDbContext dbContext) : BaseRepository<User, Guid>(dbContext), IUserRepository
{
    public async Task<User> GetByLoginAsync(string login)
    {
        return await _table
            .Include(u => u.UserProfile)
            .Include(u => u.UserToken)
            .Include(u => u.Subscription)
            .FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _table.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> GetUserWithProfileById(Guid userId)
    {
        return await _table
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}
