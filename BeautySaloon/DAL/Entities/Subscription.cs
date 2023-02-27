using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Entities;

public class Subscription : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private Subscription()
    {
    }

    public Subscription(
        string name,
        decimal price,
        int? lifeTime)
    {
        Name = name;
        Price = price;
        LifeTime = lifeTime;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int? LifeTime { get; set; }

    public decimal Price { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public List<SubscriptionCosmeticService> SubscriptionCosmeticServices { get; set; } = new List<SubscriptionCosmeticService>();

    public void AddCosmeticServices(IEnumerable<SubscriptionCosmeticService> cosmeticServices)
    {
        SubscriptionCosmeticServices.Clear();
        SubscriptionCosmeticServices.AddRange(cosmeticServices);
    }

    public void Update(
        string name,
        decimal price,
        int? lifeTime)
    {
        Name = name;
        Price = price;
        LifeTime = lifeTime;
    }
}
