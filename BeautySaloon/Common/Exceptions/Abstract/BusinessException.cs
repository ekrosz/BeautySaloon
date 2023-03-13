using System.Net;

namespace BeautySaloon.Common.Exceptions.Abstract;

public abstract class BusinessExceptions : Exception
{
    public HttpStatusCode StatusCode { get; private set; }

    public BusinessExceptions(HttpStatusCode statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}

