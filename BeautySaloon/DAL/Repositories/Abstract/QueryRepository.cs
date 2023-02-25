using BeautySaloon.DAL.Entities.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.DAL.Repositories.Abstract;
public class QueryRepository<TEntity> : ReadRepository<TEntity>, IQueryRepository<TEntity> where TEntity : class, IEntity
{
    private readonly BeautySaloonDbContext _dbContext;

    protected override IQueryable<TEntity> Query => _dbContext.Set<TEntity>()
        .IgnoreAutoIncludes()
        .AsNoTracking()
        .AsSplitQuery();

    public QueryRepository(BeautySaloonDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
