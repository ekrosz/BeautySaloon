using BeautySaloon.Core.Api.SmartPay.Dto.CreateInvoice;
using BeautySaloon.Core.Api.SmartPay.Dto.GetInvoice;
using BeautySaloon.Core.Api.SmartPay.Dto.ProcessInvoice;
using Refit;

namespace BeautySaloon.Core.Api.SmartPay;

[Headers("Content-Type: application/json")]
public interface ISmartPayHttpClient
{
    [Post("/smartpay/v1/invoices")]
    Task<CreateInvoiceResponseDto> CreateInvoiceAsync([Body] CreateInvoiceRequestDto request, CancellationToken cancellationToken);

    [Post("/smartpay/v1/invoices/{id}")]
    Task<ProcessInvoiceResponseDto> ProcessInvoiceAsync(string id, [Body] ProcessInvoiceRequestDto request, CancellationToken cancellationToken);

    [Get("/smartpay/v1/invoices/{id}")]
    Task<GetInvoiceResponseDto> GetInvoiceAsync(string id, CancellationToken cancellationToken);
}
