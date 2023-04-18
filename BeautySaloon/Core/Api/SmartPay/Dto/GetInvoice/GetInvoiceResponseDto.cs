using Newtonsoft.Json;

namespace BeautySaloon.Core.Api.SmartPay.Dto.GetInvoice;

public record GetInvoiceResponseDto
{
    [JsonProperty("invoice_status")]
    public string InvoiceStatus { get; init; } = default!;
}
