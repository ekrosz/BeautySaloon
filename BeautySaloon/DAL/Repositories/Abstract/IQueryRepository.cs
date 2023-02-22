using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Repositories.Abstract;

public interface IQueryRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IEntity
{
}
