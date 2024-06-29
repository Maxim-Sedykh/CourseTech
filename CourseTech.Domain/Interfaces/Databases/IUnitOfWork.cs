using Microsoft.EntityFrameworkCore.Storage;

namespace CourseTech.Domain.Interfaces.Databases;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
}
