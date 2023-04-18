using Newtonsoft.Json;

namespace BeautySaloon.Core.Api.SmartPay.Dto.ProcessInvoice;

public record ProcessInvoiceRequestDto
{
    [JsonProperty("user_id")]
    public UserRequestDto UserId { get; init; } = default!;

    [JsonProperty("operations")]
    public IReadOnlyCollection<OperationRequestDto> Operations { get; init; } = Array.Empty<OperationRequestDto>();

    public record UserRequestDto
    {
        [JsonProperty("partner_client_id")]
        public Guid PartnerClientId { get; init; }
    }

    public record OperationRequestDto
    {
        [JsonProperty("operation")]
        public string Operation { get; init; } = default!;

        [JsonProperty("code")]
        public string Code { get; init; } = default!;

        [JsonProperty("value")]
        public string Value { get; init; } = default!;
    }
}
