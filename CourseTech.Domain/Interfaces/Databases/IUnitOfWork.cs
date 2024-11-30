using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace CourseTech.Domain.Interfaces.Databases;

/// <summary>
/// Позволяет реализовать транзакцию EF Core.
/// </summary>
public interface IUnitOfWork : IStateSaveChanges
{
    /// <summary>
    /// Создание транзакции.
    /// </summary>
    /// <returns></returns>
    Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
}
