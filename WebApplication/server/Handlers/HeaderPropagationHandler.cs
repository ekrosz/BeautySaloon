using BeautySaloon.Api.Dto.Requests.Auth;
using BeautySaloon.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Handlers;

public class HeaderPropagationHandler : DelegatingHandler
{
    private const string TokenType = "Bearer";

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HeaderPropagationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var scope = _serviceScopeFactory.CreateScope();

        var tokenStorage = scope.ServiceProvider.GetRequiredService<ITokenStorage>();

        var accessToken = tokenStorage.GetAccessToken();

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenType, accessToken);

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized && response.StatusCode != System.Net.HttpStatusCode.Forbidden)
        {
            return response;
        }

        var authHttpClient = scope.ServiceProvider.GetRequiredService<IAuthHttpClient>();

        var refreshToken = tokenStorage.GetRefreshToken();

        try
        {
            var authResponse = await authHttpClient.AuthorizeAsync(new AuthorizeByRefreshTokenRequestDto { RefreshToken = refreshToken! });

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(TokenType, authResponse.AccessToken);

            response = await base.SendAsync(request, cancellationToken);

            return response;
        }
        catch
        {
            tokenStorage.Clear();

            return response;
        }
    }
}
