using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class AnswerRepository(CourseDbContext dbContext) : BaseRepository<Answer, long>(dbContext), IAnswerRepository
    {
    }
}
