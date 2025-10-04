using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;

namespace CourseTech.DAL.Repositories
{
    public class SubscriptionRepository(CourseDbContext dbContext) : BaseRepository<Subscription, int>(dbContext), ISubscriptionRepository
    {
    }
}
