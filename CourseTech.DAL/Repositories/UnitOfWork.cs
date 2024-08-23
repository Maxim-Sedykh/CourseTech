using CourseTech.Domain.Entities;
using CourseTech.Domain.Interfaces.Databases;
using CourseTech.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CourseTech.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CourseDbContext _context;

    public IBaseRepository<User> Users { get; set; }

    public IBaseRepository<Review> Reviews { get; set; }

    public IBaseRepository<UserProfile> UserProfiles { get; set; }

    public UnitOfWork(CourseDbContext context, IBaseRepository<User> users, IBaseRepository<Review> reviews)
    {
        _context = context;
        Users = users;
        Reviews = reviews;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
