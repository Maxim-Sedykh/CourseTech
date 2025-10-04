using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class UserRepository(CourseDbContext dbContext) : BaseRepository<User, Guid>(dbContext), IUserRepository
    {
    }
}
