﻿using BeautySaloon.DAL.Entities.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using System.Linq.Expressions;

namespace BeautySaloon.DAL.Repositories.Abstract;

public interface IQueryRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IEntity
{
    Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<PageResponseDto<TEntity>> GetPageAsync<TKey>(
        PageRequestDto request,
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TKey>> sortProperty,
        bool asc = true,
        CancellationToken cancellationToken = default);
}
