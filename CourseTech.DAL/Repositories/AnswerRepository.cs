using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class UserTokenRepository : BaseRepository<UserToken, long>, IUserTokenRepository
    {
        public UserTokenRepository(CourseDbContext dbContext) : base(dbContext) { }
    }
}
