using BeautySaloon.Core.Api.SmartPay.Dto.Common;
using Newtonsoft.Json;

namespace BeautySaloon.Core.Api.SmartPay.Dto.ProcessInvoice;

public record ProcessInvoiceResponseDto
{
    [JsonProperty("form_url")]
    public string FormUrl { get; init; } = string.Empty;

    [JsonProperty("error")]
    public ErrorResponseDto Error { get; init; } = new();
}
