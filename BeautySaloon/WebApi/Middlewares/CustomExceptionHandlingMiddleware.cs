using BeautySaloon.Api;
using BeautySaloon.Common.Exceptions.Abstract;
using FluentValidation;
using Newtonsoft.Json;
using System.Net;

namespace BeautySaloon.WebApi.Middlewares;

public class CustomExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public CustomExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;

            response.ContentType = "application/json";

            ErrorDetails errorDetails = null!;

            switch (error)
            {
                case BusinessExceptions e:
                    response.StatusCode = (int)e.StatusCode;

                    errorDetails = new ErrorDetails
                    {
                        StatusCode = e.StatusCode,
                        ErrorMessage = e.Message,
                        ErrorType = e.GetType().Name,
                        ErrorCatrgory = e.GetType().BaseType!.Name,
                        Data = e.Data
                    };

                    break;
                case ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;

                    errorDetails = new ErrorDetails
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessage = e.Message,
                        ErrorType = e.GetType().Name,
                        ErrorCatrgory = e.GetType().BaseType!.Name,
                        Data = e.Data
                    };

                    break;
                default:
                    var ex = new UnexpectedException(error.Message);

                    response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    errorDetails = new ErrorDetails
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        ErrorMessage = ex.Message,
                        ErrorType = ex.GetType().Name,
                        ErrorCatrgory = ex.GetType().BaseType == typeof(Exception)
                            ? nameof(UnexpectedException)
                            : ex.GetType().BaseType!.Name
                    };

                    break;
            }

            var result = JsonConvert.SerializeObject(errorDetails);

            await response.WriteAsync(result);
        }
    }

}
