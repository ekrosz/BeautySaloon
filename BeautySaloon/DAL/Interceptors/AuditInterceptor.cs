using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautySaloon.DAL.Interceptors;

public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserProvider _currentUserProvider;

    private static readonly IReadOnlyCollection<EntityState> AllowStates = new[]
    {
        EntityState.Added,
        EntityState.Modified,
        EntityState.Deleted
    };

    public AuditInterceptor(ICurrentUserProvider currentUserProvider)
    {
        _currentUserProvider = currentUserProvider;
    }

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
            .Where(x => x.Entity is IAuditable && AllowStates.Contains(x.State))
            .ToArray();

        if (!auditableEntiries.Any())
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var utcNow = DateTime.UtcNow;
        var currentUserId = _currentUserProvider.GetUserId();

        foreach (var entry in auditableEntiries)
        {
            entry.CurrentValues[nameof(IAuditable.UpdatedOn)] = utcNow;
            entry.CurrentValues[nameof(IAuditable.UserModifierId)] = currentUserId;
        }

        foreach (var entry in auditableEntiries.Where(x => x.State == EntityState.Added))
        {
            entry.CurrentValues[nameof(IAuditable.CreatedOn)] = utcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
