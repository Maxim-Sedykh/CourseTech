using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class CategoryRepository(CourseDbContext dbContext) : BaseRepository<Category, int>(dbContext), ICategoryRepository;
}
