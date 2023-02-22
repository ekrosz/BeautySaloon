using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Repositories.Abstract;
public class QueryRepository<TEntity> : ReadRepository<TEntity>, IQueryRepository<TEntity> where TEntity : class, IEntity
{
}
