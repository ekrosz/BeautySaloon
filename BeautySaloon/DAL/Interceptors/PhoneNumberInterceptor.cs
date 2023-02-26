using BeautySaloon.Common.Utils;
using BeautySaloon.DAL.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BeautySaloon.DAL.Interceptors;

public class PhoneNumberInterceptor : SaveChangesInterceptor
{
    private static readonly IReadOnlyCollection<EntityState> AllowStates = new[]
    {
        EntityState.Added,
        EntityState.Modified
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

        var processingEntiries = eventData.Context.ChangeTracker.Entries()
            .Where(x => x.Entity is IHasPhoneNumber && AllowStates.Contains(x.State))
            .ToArray();

        foreach (var entry in processingEntiries)
        {
            entry.CurrentValues[nameof(IHasPhoneNumber.PhoneNumber)] = PhoneNumberUtilities.Normilize(((IHasPhoneNumber)entry.Entity).PhoneNumber);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
