using BeautySaloon.Core.Dto.Requests.Order;

namespace BeautySaloon.Core.Services.Contracts;

public interface IOrderService
{
    Task CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default);
}
