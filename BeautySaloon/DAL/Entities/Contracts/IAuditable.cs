namespace BeautySaloon.DAL.Entities.Contracts;

public interface IAuditable
{
    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }

    public Guid UserModifierId { get; set; }
}
