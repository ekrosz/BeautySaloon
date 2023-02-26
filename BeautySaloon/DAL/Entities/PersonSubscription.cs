using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Entities;

public class PersonSubscription : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private PersonSubscription()
    {
    }

    public PersonSubscription(Guid personId, Guid subscriptionId)
    {
        PersonId = personId;
        SubscriptionId = subscriptionId;
    }

    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public Guid SubscriptionId { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public Subscription Subscription { get; set; } = default!;
}
