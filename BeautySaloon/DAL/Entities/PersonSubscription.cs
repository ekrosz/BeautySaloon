using BeautySaloon.Common.Exceptions;
using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.DAL.Entities;

public class PersonSubscription : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private PersonSubscription()
    {
    }

    public PersonSubscription(Guid subscriptionCosmeticServiceId, SubscriptionCosmeticServiceSnapshot subscriptionCosmeticServiceSnapshot)
    {
        SubscriptionCosmeticServiceId = subscriptionCosmeticServiceId;
        SubscriptionCosmeticServiceSnapshot = subscriptionCosmeticServiceSnapshot;
        Status = PersonSubscriptionStatus.NotPaid;
    }

    public Guid Id { get; set; }

    public Guid SubscriptionCosmeticServiceId { get; set; }

    public PersonSubscriptionStatus Status { get; set; }

    public SubscriptionCosmeticServiceSnapshot SubscriptionCosmeticServiceSnapshot { get; set; } = default!;

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public Order Order { get; set; } = default!;

    public void ValidateStatusOrThrow()
    {
        if (Status == PersonSubscriptionStatus.NotPaid)
        {
            throw new PersonSubscriptionNotPaidException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionStatus.Cancelled)
        {
            throw new PersonSubscriptionWasCancelledException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionStatus.InProgress)
        {
            throw new PersonSubscriptionAlreadyInProgressException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionStatus.Completed)
        {
            throw new PersonSubscriptionAlreadyCompletedException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }

        if (Status == PersonSubscriptionStatus.Overdue)
        {
            throw new PersonSubscriptionOverdueException(
                SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot.Name);
        }
    }

    public void Overdue()
    {
        ValidateStatusOrThrow();

        Status = PersonSubscriptionStatus.Overdue;
    }
}
