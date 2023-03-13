﻿using BeautySaloon.Api.Dto.Requests.Person;
using BeautySaloon.Api.Dto.Responses.Person;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IPersonClient
{
    [Post("/api/persons")]
    Task CreateAsync([Body] CreatePersonRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/persons/{id}")]
    Task UpdateAsync(Guid id, [Body] UpdatePersonRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/persons/{id}")]
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/persons/{id}")]
    Task<GetPersonResponseDto> GetAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/persons")]
    Task<PageResponseDto<GetPersonListItemResponseDto>> GetListAsync([Query] GetPersonListRequestDto request, CancellationToken cancellationToken = default);
}