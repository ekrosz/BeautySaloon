using BeautySaloon.Core.Api.SmartPay.Dto.Common;
using Newtonsoft.Json;

namespace BeautySaloon.Core.Api.SmartPay.Dto.CreateInvoice;

public record CreateInvoiceResponseDto
{
    [JsonProperty("invoice_id")]
    public string? InvoiceId { get; init; }

    [JsonProperty("error")]
    public ErrorResponseDto Error { get; init; } = new();
}
