using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeautySaloon.DAL.Repositories.Abstract;
public class QueryRepository<TEntity> : ReadRepository<TEntity>, IQueryRepository<TEntity> where TEntity : class, IEntity
{
    private readonly BeautySaloonDbContext _dbContext;

    protected override IQueryable<TEntity> Query => _dbContext.Set<TEntity>()
        .AsNoTracking()
        .AsSplitQuery();

    public QueryRepository(BeautySaloonDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PageResponseDto<TEntity>> GetPageAsync<TKey>(
        PageRequestDto request,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> sortProperty,
        bool asc = true,
        CancellationToken cancellationToken = default)
    {
        var items = asc
            ? Query.Where(predicate).OrderBy(sortProperty)
            : Query.Where(predicate).OrderByDescending(sortProperty);

        var page = await items.Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var totalCount = await Query.CountAsync(predicate, cancellationToken);

        return new PageResponseDto<TEntity>(page, totalCount);
    }
}
