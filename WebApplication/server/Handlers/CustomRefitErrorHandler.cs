using BeautySaloon.Api;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Handlers;

public class CustomRefitErrorHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            throw new CustomApiException(new ErrorDetails
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                ErrorMessage = "Пользователь не авторизован"
            });
        }

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            throw new CustomApiException(new ErrorDetails
            {
                StatusCode = System.Net.HttpStatusCode.Forbidden,
                ErrorMessage = "Доступ к содержимому запрещен"
            });
        }

        var rawErrorDetails = await response.Content.ReadAsStringAsync(cancellationToken);

        var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(rawErrorDetails)
            ?? throw new InvalidOperationException();

        throw new CustomApiException(errorDetails);
    }
}
