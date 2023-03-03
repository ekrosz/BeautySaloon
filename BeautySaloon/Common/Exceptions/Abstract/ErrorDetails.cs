using System.Collections;
using System.Net;

namespace BeautySaloon.Common.Exceptions.Abstract;

public record ErrorDetails
{
    public string ErrorMessage { get; init; } = string.Empty;

    public string ErrorType { get; init; } = string.Empty;

    public string ErrorCatrgory { get; init; } = string.Empty;

    public HttpStatusCode StatusCode { get; init; }

    public IDictionary Data { get; init; } = new Dictionary<string, object>();
}

