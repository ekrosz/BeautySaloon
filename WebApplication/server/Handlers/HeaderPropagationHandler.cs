using BeautySaloon.Api.Dto.Requests.Auth;
using BeautySaloon.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Handlers;

public class HeaderPropagationHandler : DelegatingHandler
{
    private const string TokenType = "Bearer";

    private readonly IServiceProvider _serviceScopeFactory;

    public HeaderPropagationHandler(IServiceProvider serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //var scope = _serviceScopeFactory.CreateScope();

        var jsRuntime = _serviceScopeFactory.GetRequiredService<IJSRuntime>();

        var accessToken = await jsRuntime.InvokeAsync<string>("localStorage.getItem", Constants.AccessTokenKey);

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenType, accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized && response.StatusCode != System.Net.HttpStatusCode.Forbidden)
        {
            return response;
        }

        var authHttpClient = _serviceScopeFactory.GetRequiredService<IAuthHttpClient>();

        var refreshToken = await jsRuntime.InvokeAsync<string>("localStorage.getItem", Constants.RefreshTokenKey);

        try
        {
            var authResponse = await authHttpClient.AuthorizeAsync(new AuthorizeByRefreshTokenRequestDto { RefreshToken = refreshToken! });

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenType, authResponse.AccessToken);

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }
        catch
        {
            return response;
        }
    }
}
