using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BeautySaloon.Core.Settings;

public record AuthenticationSettings
{
    public long AccessTokenLifetime { get; init; }

    public long RefreshTokenLifetime { get; init; }

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string AccessSecret { get; init; } = string.Empty;

    public string RefreshSecret { get; init; } = string.Empty;

    public SymmetricSecurityKey AccessSecurityKey
        => new(Encoding.UTF8.GetBytes(AccessSecret));

    public byte[] RefreshSecurityBytesKey
        => Encoding.UTF8.GetBytes(RefreshSecret);
}
