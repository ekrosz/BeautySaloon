namespace BeautySaloon.DAL.Uow;

public class UnitOfWork : IUnitOfWork
{
    private readonly BeautySaloonDbContext _dbContext;

    public UnitOfWork(BeautySaloonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
