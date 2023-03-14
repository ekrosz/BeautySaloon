namespace WebApp.Handlers;

public interface ITokenStorage
{
    public void Save(string accessToken, string refreshToken);

    public string? GetAccessToken();

    public string? GetRefreshToken();

    public void Clear();
}
