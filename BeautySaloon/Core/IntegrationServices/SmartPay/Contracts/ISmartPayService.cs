using BeautySaloon.Core.IntegrationServices.SmartPay.Dto;

namespace BeautySaloon.Core.IntegrationServices.SmartPay.Contracts;

public interface ISmartPayService
{
    public Task<CreatePaymentRequestResponseDto> CreatePaymentRequestAsync(Guid orderId, CancellationToken cancellationToken = default);

    public Task<GetPaymentRequestResponseDto> GetPaymentRequestStatusAsync(Guid orderId, CancellationToken cancellationToken = default);
}
