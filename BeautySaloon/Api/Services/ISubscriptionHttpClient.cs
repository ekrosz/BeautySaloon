using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.Api.Dto.Responses.Subscription;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface ISubscriptionHttpClient
{
    [Post("/api/subscriptions")]
    Task CreateAsync([Body] CreateSubscriptionRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/subscriptions/{id}")]
    Task UpdateAsync(Guid id, [Body] UpdateSubscriptionRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/subscriptions/{id}")]
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/subscriptions/{id}")]
    Task<GetSubscriptionResponseDto> GetAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/subscriptions")]
    Task<PageResponseDto<GetSubscriptionListItemResponseDto>> GetListAsync([Query] GetSubscriptionListRequestDto request, CancellationToken cancellationToken = default);
}
