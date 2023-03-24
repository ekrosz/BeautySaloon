using BeautySaloon.Api.Dto.Responses.Auth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication.Handlers;

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

        var authRawResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        var authResponse = JsonConvert.DeserializeObject<AuthorizeResponseDto>(authRawResponse)
            ?? throw new InvalidOperationException();

        //var scope = _serviceScopeFactory.CreateScope();

        //var jsRuntime = scope.ServiceProvider.GetRequiredService<IJSRuntime>();

        //await jsRuntime.InvokeVoidAsync("localStorage.setItem", Constants.AccessTokenKey, authResponse.AccessToken);
        //await jsRuntime.InvokeVoidAsync("localStorage.setItem", Constants.RefreshTokenKey, authResponse.RefreshToken);

        return response;
    }
}
