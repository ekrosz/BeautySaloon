using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.Api.Dto.Responses.Order;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IOrderHttpClient
{
    [Post("/api/orders")]
    Task CreateAsync([Body] CreateOrderRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/orders/{id}")]
    Task UpdateAsync(Guid id, [Body] UpdateOrderRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/orders/{id}/pay")]
    Task PayAsync(Guid id, [Body] PayOrderRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/orders/{id}/cancel")]
    Task CancelAsync(Guid id, [Body] CancelOrderRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/orders")]
    Task<PageResponseDto<GetOrderResponseDto>> GetListAsync([Query] GetOrderListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/orders/{id}")]
    Task<GetOrderResponseDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
