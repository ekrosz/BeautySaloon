using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Entities;

public class SubscriptionCosmeticService : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private SubscriptionCosmeticService()
    {
    }

    public SubscriptionCosmeticService(Guid cosmeticServiceId, int count)
    {
        CosmeticServiceId = cosmeticServiceId;
        Count = count;
    }

    public Guid Id { get; set; }

    public Guid CosmeticServiceId { get; set; }

    public Guid SubscriptionId { get; set; }

    public int Count { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public CosmeticService CosmeticService { get; set; } = default!;

    public Subscription Subscription { get; set; } = default!;
}
