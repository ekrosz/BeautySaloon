using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Repositories.Abstract;

public class WriteRepository<TEntity> : ReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : class, IEntity
{
    private readonly BeautySaloonDbContext _dbContext;

    public WriteRepository(BeautySaloonDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await _dbContext.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await _dbContext.AddRangeAsync(entities, cancellationToken);

    public void Delete(TEntity entity)
        => _dbContext.Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities)
        => _dbContext.RemoveRange(entities);

    public void Update(TEntity entity)
        => _dbContext.Update(entity);

    public void UpdateRange(IEnumerable<TEntity> entities)
        => _dbContext.UpdateRange(entities);
}
