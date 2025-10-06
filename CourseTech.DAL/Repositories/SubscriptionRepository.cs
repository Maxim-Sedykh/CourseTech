using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CourseTech.DAL.Repositories
{
    public class SubscriptionRepository(CourseDbContext dbContext) : BaseRepository<Subscription, int>(dbContext), ISubscriptionRepository
    {
        public async Task<Subscription> GetByNameAsync(string name)
        {
            return await _table.FirstOrDefaultAsync(s => s.Name == name);
        }
    }
}
