using BeautySaloon.DAL.Entities.Contracts;

namespace BeautySaloon.DAL.Repositories.Abstract;

public class WriteRepository<TEntity> : ReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : class, IEntity
{
}
