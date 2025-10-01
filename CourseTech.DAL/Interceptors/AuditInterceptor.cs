using CourseTech.Domain.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CourseTech.DAL.Interceptors;

/// <summary>
/// Перехватчик запросов в базу.
/// Служит для того чтобы заполнять свойства интерфейса IAuditable (CreatedAt, UpdatedAt).
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var dbContext = eventData.Context;
        if (dbContext == null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = dbContext.ChangeTracker.Entries<IAuditable>()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        var createdEntries = dbContext.ChangeTracker.Entries<ICreatable>()
            .Where(x => x.State == EntityState.Added);

        DateTime currentTimeUtc = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(x => x.CreatedAt).CurrentValue = currentTimeUtc;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property(x => x.UpdatedAt).CurrentValue = currentTimeUtc;
            }
        }

        foreach (var createdEntry in createdEntries)
        {

        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
