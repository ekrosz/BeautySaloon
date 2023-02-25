namespace BeautySaloon.DAL.Entities.Contracts;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }

    public void Delete() => IsDeleted = true;
}
