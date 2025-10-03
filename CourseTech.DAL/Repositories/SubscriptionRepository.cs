using CourseTech.DAL.Repositories.Base;
using CourseTech.Domain.Entities.UserRelated;
using CourseTech.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.DAL.Repositories
{
    public class UserTokenRepository : BaseRepository<UserToken, long>, IUserTokenRepository
    {
        public UserTokenRepository(CourseDbContext dbContext) : base(dbContext) { }
    }
}
