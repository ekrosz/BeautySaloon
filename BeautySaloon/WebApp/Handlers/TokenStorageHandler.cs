using BeautySaloon.Api.Dto.Responses.Auth;
using Microsoft.IO;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WebApp.Handlers;

public class TokenStorageHandler : DelegatingHandler
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TokenStorageHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return response;
        }

        var scope = _serviceScopeFactory.CreateScope();

        var tokenStorage = scope.ServiceProvider.GetRequiredService<ITokenStorage>();

        var authRawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        var authResponse = JsonConvert.DeserializeObject<AuthorizeResponseDto>(authRawResponse)
            ?? throw new InvalidOperationException();

        tokenStorage.Save(authResponse.AccessToken, authResponse.RefreshToken);

        return response;
    }
}
