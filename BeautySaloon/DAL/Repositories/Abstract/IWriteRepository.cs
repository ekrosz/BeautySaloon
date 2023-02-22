using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Repositories.Abstract;

public interface IWriteRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IEntity
{
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    public Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    public void Update(TEntity entity);

    public void UpdateRange(IEnumerable<TEntity> entities);

    public void Delete(TEntity entity);

    public void DeleteRange(IEnumerable<TEntity> entities);
}
