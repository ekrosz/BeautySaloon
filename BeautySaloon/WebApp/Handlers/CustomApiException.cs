using BeautySaloon.Api;

namespace WebApp.Handlers;

public class CustomApiException : Exception
{
    public CustomApiException(ErrorDetails errorDetails)
        : base($"Ошибка {(int)errorDetails.StatusCode}")
    {
        Details = errorDetails;
    }

    public ErrorDetails Details { get; }
}
