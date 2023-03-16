using BeautySaloon.Api;
using System;

namespace WebApplication.Handlers;

public class CustomApiException : Exception
{
    public CustomApiException(ErrorDetails errorDetails)
        : base($"Ошибка {(int)errorDetails.StatusCode}")
    {
        Details = errorDetails;
    }

    public ErrorDetails Details { get; }
}
