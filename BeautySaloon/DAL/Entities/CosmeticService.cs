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
        int executeTimeInMinutes)
    {
        Name = name;
        Description = description;
        ExecuteTimeInMinutes = executeTimeInMinutes;
    }

    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int ExecuteTimeInMinutes { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }

    public void Update(
        string name,
        string description,
        int executeTimeInMinutes)
    {
        Name = name;
        Description = description;
        ExecuteTimeInMinutes = executeTimeInMinutes;
    }
}
