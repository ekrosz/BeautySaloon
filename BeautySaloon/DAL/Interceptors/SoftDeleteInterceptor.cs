using BeautySaloon.DAL.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautySaloon.DAL.Interceptors;
public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    private static readonly IReadOnlyCollection<EntityState> AllowStates = new[]
    {
        EntityState.Deleted
    };

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var auditableEntiries = eventData.Context.ChangeTracker.Entries()
            .Where(x => x.Entity is ISoftDeletable && AllowStates.Contains(x.State))
            .ToArray();

        foreach (var entry in auditableEntiries)
        {
            entry.State = EntityState.Unchanged;
            entry.CurrentValues[nameof(ISoftDeletable.IsDeleted)] = true;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
