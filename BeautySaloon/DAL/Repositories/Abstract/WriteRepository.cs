using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Repositories.Abstract;

public class WriteRepository<TEntity> : ReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : class, IEntity
{
    public WriteRepository(BeautySaloonDbContext dbContext)
        : base(dbContext)
    {
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        => await DbSet.AddRangeAsync(entities, cancellationToken);

    public void Delete(TEntity entity)
    {
        if (entity is ISoftDeletable deletableEntity)
        {
            deletableEntity.Delete();
            return;
        }

        DbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        entities.Where(e => e is ISoftDeletable)
            .ToList()
            .ForEach(e => ((ISoftDeletable)e).Delete());

        DbSet.RemoveRange(entities.Where(e => e is not ISoftDeletable));
    }
}
