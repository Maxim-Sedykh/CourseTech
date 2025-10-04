using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class SessionRepository(CourseDbContext dbContext) : BaseRepository<Session, long>(dbContext), ISessionRepository
    {
    }
}
