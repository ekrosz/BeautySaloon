using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.DAL.Entities;

public class PersonSubscription : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private PersonSubscription()
    {
    }

    public PersonSubscription(Guid subscriptionCosmeticServiceId)
    {
        SubscriptionCosmeticServiceId = subscriptionCosmeticServiceId;
        Status = PersonSubscriptionStatus.NotPaid;
    }

    public Guid Id { get; set; }

    public Guid SubscriptionCosmeticServiceId { get; set; }

    public Guid OrderId { get; set; }

    public Guid? AppointmentId { get; set; }

    public PersonSubscriptionStatus Status { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public Order Order { get; set; } = default!;

    public SubscriptionCosmeticService SubscriptionCosmeticService { get; set; } = default!;
}
