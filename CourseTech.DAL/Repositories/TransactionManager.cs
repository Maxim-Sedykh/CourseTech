using CourseTech.Domain.Interfaces.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace CourseTech.DAL.Repositories;

/// <summary>
/// Сервис для управления транзакциями EF Core
/// </summary>
/// <param name="dbContext"></param>
public class TransactionManager(CourseDbContext dbContext) : ITransactionManager
{
    /// <inheritdoc/>
    public async Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return await dbContext.Database.BeginTransactionAsync(isolationLevel);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }
}
