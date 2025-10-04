using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class QuestionRepository(CourseDbContext dbContext) : BaseRepository<Question, int>(dbContext), IQuestionRepository
    {
    }
}
