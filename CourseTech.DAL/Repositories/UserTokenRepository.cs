using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class UserTokenRepository(CourseDbContext dbContext) : BaseRepository<UserToken, long>(dbContext), IUserTokenRepository
    {
    }
}
