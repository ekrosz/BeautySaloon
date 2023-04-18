using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class SmartPayApiException : BusinessExceptions
{
    public SmartPayApiException(string userMessage, string errorDescription, string errorCode)
        : base(HttpStatusCode.Conflict, $"{userMessage}\n{errorDescription}\n{errorCode}")
    {
    }
}
