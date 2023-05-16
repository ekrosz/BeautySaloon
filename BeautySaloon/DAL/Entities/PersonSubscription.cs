using BeautySaloon.Common.Exceptions;
using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.DAL.Entities;

public class PersonSubscription : IEntity
{
    [Obsolete("For EF")]
    private PersonSubscription()
    {
    }

    public PersonSubscription(SubscriptionCosmeticServiceSnapshot subscriptionCosmeticServiceSnapshot)
    {
        SubscriptionCosmeticServiceSnapshot = subscriptionCosmeticServiceSnapshot;
        Status = PersonSubscriptionCosmeticServiceStatus.NotPaid;
    }

    public Guid Id { get; set; }

    public PersonSubscriptionCosmeticServiceStatus Status { get; set; }

    public SubscriptionCosmeticServiceSnapshot SubscriptionCosmeticServiceSnapshot { get; set; } = default!;

    public Guid OrderId { get; set; }

    public Order Order { get; set; } = default!;

    public void ValidateStatusOrThrow()
    {
        if (Status == PersonSubscriptionCosmeticServiceStatus.NotPaid)
        {
            throw new PersonSubscriptionNotPaidException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionCosmeticServiceStatus.Cancelled)
        {
            throw new PersonSubscriptionWasCancelledException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionCosmeticServiceStatus.InProgress)
        {
            throw new PersonSubscriptionAlreadyInProgressException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionCosmeticServiceStatus.Completed)
        {
            throw new PersonSubscriptionAlreadyCompletedException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionCosmeticServiceStatus.Overdue)
        {
            throw new PersonSubscriptionOverdueException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }
    }

    public void Overdue()
    {
        ValidateStatusOrThrow();

        Status = PersonSubscriptionCosmeticServiceStatus.Overdue;
    }
}
