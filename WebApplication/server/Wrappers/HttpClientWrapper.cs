using BeautySaloon.Api.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebApplication.Handlers;

namespace WebApplication.Wrappers;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly IJSRuntime _jsRuntime;

    private readonly IAuthHttpClient _authHttpClient;

    private readonly NotificationService _notificationService;

    private readonly NavigationManager _navigationManager;

    public HttpClientWrapper(
        IJSRuntime jsRuntime,
        IAuthHttpClient authHttpClient,
        NotificationService notificationService,
        NavigationManager navigationManager)
    {
        _jsRuntime = jsRuntime;
        _authHttpClient = authHttpClient;
        _notificationService = notificationService;
        _navigationManager = navigationManager;
    }

    public async Task<bool> SendAsync(Func<string, Task> call)
    {
        var accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", Constants.AccessTokenKey) ?? string.Empty;

        try
        {
            await call(accessToken);

            return true;
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

            return false;
        }
    }

    public async Task<T?> SendAsync<T>(Func<string, Task<T>> call)
    {
        var accessToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", Constants.AccessTokenKey) ?? string.Empty;

        try
        {
            var response = await call(accessToken);

            return response;
        }
        catch (CustomApiException ex)
        {
            if (ex.Details.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", Constants.RefreshTokenKey) ?? string.Empty;

                try
                {
                    var authResponse = await _authHttpClient.AuthorizeAsync(new BeautySaloon.Api.Dto.Requests.Auth.AuthorizeByRefreshTokenRequestDto { RefreshToken = refreshToken }, CancellationToken.None);

                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", Constants.AccessTokenKey, $"{Constants.TokenType} {authResponse.AccessToken}");
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", Constants.RefreshTokenKey, authResponse.RefreshToken);

                    var response = await call(authResponse.AccessToken);

                    return response;
                }
                catch (CustomApiException)
                {
                    await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", Constants.AccessTokenKey);
                    await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", Constants.RefreshTokenKey);
                }
            }

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
