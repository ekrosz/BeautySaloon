namespace BeautySaloon.DAL.Providers;

public interface ICurrentUserProvider
{
    public Guid UserId { get; set; }
}
