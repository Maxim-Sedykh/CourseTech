using CourseTech.Domain.Interfaces.Databases;
using Microsoft.EntityFrameworkCore.Storage;

namespace CourseTech.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CourseDbContext _context;

    public UnitOfWork(CourseDbContext context)
    {
        _context = context;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
