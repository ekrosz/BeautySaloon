using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Entities;

public class SubscriptionServiceInspection : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private SubscriptionServiceInspection()
    {
    }

    public SubscriptionServiceInspection(int count)
    {
        Count = count;
    }

    public Guid Id { get; set; }

    public int Count { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }
}
