using System.Collections;
using System.Net;

namespace BeautySaloon.Api;

public record ErrorDetails
{
    public string ErrorMessage { get; init; } = string.Empty;

    public string ErrorType { get; init; } = string.Empty;

    public string ErrorCatrgory { get; init; } = string.Empty;

    public HttpStatusCode StatusCode { get; init; }

    public IDictionary Data { get; init; } = new Dictionary<string, object>();
}

