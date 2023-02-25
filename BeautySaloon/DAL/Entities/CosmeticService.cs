using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Entities;

public class CosmeticService : IEntity, IAuditable
{
    [Obsolete("For EF")]
    private CosmeticService()
    {
    }

    public CosmeticService(
        string name,
        string description,
        int executeTime)
    {
        Name = name;
        Description = description;
        ExecuteTime = executeTime;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int ExecuteTime { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public List<SubscriptionServiceInspection> SubscriptionServiceInspections { get; set; } = new List<SubscriptionServiceInspection>();
}
