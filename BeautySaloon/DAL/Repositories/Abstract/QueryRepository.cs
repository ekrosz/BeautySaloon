using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeautySaloon.DAL.Repositories.Abstract;
public class QueryRepository<TEntity> : ReadRepository<TEntity>, IQueryRepository<TEntity> where TEntity : class, IEntity
{
    protected override IQueryable<TEntity> Query => base.Query
        .AsNoTracking()
        .AsSplitQuery();

    public QueryRepository(BeautySaloonDbContext dbContext)
        : base(dbContext)
    {
    }

    public Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Query.AnyAsync(predicate, cancellationToken);

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
