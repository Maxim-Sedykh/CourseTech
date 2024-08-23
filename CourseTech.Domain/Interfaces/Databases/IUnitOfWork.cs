using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CourseTech.Domain.Interfaces.Databases;

public interface IUnitOfWork : IStateSaveChanges
{
    Task<IDbContextTransaction> BeginTransactionAsync();

    public IBaseRepository<User> Users { get; set; }

    public IBaseRepository<UserProfile> UserProfiles { get; set; }

    public IBaseRepository<Review> Reviews { get; set; }

    public IBaseRepository<Lesson> Lessons { get; set; }

    public IBaseRepository<LessonRecord> LessonRecords { get; set; }
}
