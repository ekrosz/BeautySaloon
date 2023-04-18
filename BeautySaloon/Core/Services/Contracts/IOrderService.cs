using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.Api.Dto.Responses.Order;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Services.Contracts;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default);

    Task UpdateOrderAsync(ByIdWithDataRequestDto<UpdateOrderRequestDto> request, CancellationToken cancellationToken = default);

    Task CancelOrderAsync(ByIdWithDataRequestDto<CancelOrderRequestDto> request, CancellationToken cancellationToken = default);

    Task<PayOrderResponseDto> PayOrderAsync(ByIdWithDataRequestDto<PayOrderRequestDto> request, CancellationToken cancellationToken = default);

    Task<CheckAndUpdateOrderPaymentStatusResponseDto> CheckAndUpdateOrderPaymentStatusAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<PageResponseDto<GetOrderResponseDto>> GetOrderListAsync(GetOrderListRequestDto request, CancellationToken cancellationToken = default);

    Task<GetOrderResponseDto> GetOrderAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);
}
