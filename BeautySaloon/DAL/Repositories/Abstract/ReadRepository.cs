using BeautySaloon.DAL.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BeautySaloon.DAL.Repositories.Abstract;

public abstract class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IEntity
{
    private readonly BeautySaloonDbContext _dbContext;

    protected virtual IQueryable<TEntity> Query => _dbContext.Set<TEntity>();

    public ReadRepository(BeautySaloonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Query.AnyAsync(predicate, cancellationToken);

    public async Task<IReadOnlyCollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => await Query.Where(predicate).ToArrayAsync(cancellationToken);

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        => Query.FirstOrDefaultAsync(predicate, cancellationToken);
}
