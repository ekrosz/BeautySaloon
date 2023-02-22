namespace BeautySaloon.DAL.Providers;

public interface ICurrentUserProvider
{
    public Guid GetUserId();
}
