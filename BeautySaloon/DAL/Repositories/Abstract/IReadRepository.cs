using BeautySaloon.DAL.Entities.Contracts;
using System.Linq.Expressions;

namespace BeautySaloon.DAL.Repositories.Abstract;

public interface IReadRepository<TEntity> where TEntity : class, IEntity
{
    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IReadOnlyCollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    public Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    public Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}
