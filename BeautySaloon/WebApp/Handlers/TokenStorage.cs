namespace WebApp.Handlers;

public class TokenStorage : ITokenStorage
{
    private string? _accessToken;

    private string? _refreshToken;

    public void Clear()
    {
        _accessToken = null;
        _refreshToken = null;
    }

    public string? GetAccessToken() => _accessToken;

    public string? GetRefreshToken() => _refreshToken;

    public void Save(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
    }
}
