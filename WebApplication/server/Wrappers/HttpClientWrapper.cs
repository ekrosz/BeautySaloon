using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Handlers;

namespace WebApplication.Wrappers;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IJSRuntime _jsRuntime;

    private readonly NotificationService _notificationService;

    private readonly NavigationManager _navigationManager;

    public HttpClientWrapper(
        IJSRuntime jsRuntime,
        NotificationService notificationService,
        NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _notificationService = notificationService;
        _navigationManager = navigationManager;
    }

    public async Task SendAsync(Func<string, Task> call)
    {
        var accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", Constants.AccessTokenKey) ?? string.Empty;

        try
        {
            await call(accessToken);
        }
        catch (CustomApiException ex)
        {
            _notificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = ex.Message,
                Detail = ex.Details.ErrorMessage
            });

            if (ex.Details.StatusCode == System.Net.HttpStatusCode.Unauthorized || ex.Details.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                _navigationManager.NavigateTo("/login");
            }
        }
    }

    public async Task<T?> SendAsync<T>(Func<string, Task<T>> call)
    {
        var accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", Constants.AccessTokenKey);

        try
        {
            var response = await call(accessToken);

            return response;
        }
        catch (CustomApiException ex)
        {
            _notificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Summary = ex.Message,
                Detail = ex.Details.ErrorMessage
            });

            if (ex.Details.StatusCode == System.Net.HttpStatusCode.Unauthorized || ex.Details.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                _navigationManager.NavigateTo("/login");
            }

            return default;
        }
    }
}
