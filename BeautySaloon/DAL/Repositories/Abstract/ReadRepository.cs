using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Repositories.Abstract;

public abstract class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IEntity
{
}
