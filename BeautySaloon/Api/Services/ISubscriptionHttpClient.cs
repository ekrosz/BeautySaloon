using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.Api.Dto.Responses.Subscription;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface ISubscriptionHttpClient
{
    [Post("/api/subscriptions")]
    Task CreateAsync([Header("Authorization")] string accessToken, [Body] CreateSubscriptionRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/subscriptions/{id}")]
    Task UpdateAsync([Header("Authorization")] string accessToken, Guid id, [Body] UpdateSubscriptionRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/subscriptions/{id}")]
    Task DeleteAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/subscriptions/{id}")]
    Task<GetSubscriptionResponseDto> GetAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/subscriptions")]
    Task<PageResponseDto<GetSubscriptionListItemResponseDto>> GetListAsync([Header("Authorization")] string accessToken, [Query] GetSubscriptionListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/subscriptions/analytic")]
    Task<GetSubscriptionAnalyticResponseDto> GetAnalyticAsync([Header("Authorization")] string accessToken, [Query] GetSubscriptionAnalyticRequestDto request, CancellationToken cancellationToken = default);
}
