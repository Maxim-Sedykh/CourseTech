using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class UserProfileRepository(CourseDbContext dbContext) : BaseRepository<UserProfile, long>(dbContext), IUserProfileRepository
    {
    }
}
