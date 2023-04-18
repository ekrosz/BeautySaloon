using Newtonsoft.Json;

namespace BeautySaloon.Core.Api.SmartPay.Dto.Common;

public record ErrorResponseDto
{
    [JsonProperty("user_message")]
    public string UserMessage { get; set; } = string.Empty;

    [JsonProperty("error_description")]
    public string ErrorDescription { get; set; } = string.Empty;

    [JsonProperty("error_code")]
    public string ErrorCode { get; set; } = string.Empty;
}
