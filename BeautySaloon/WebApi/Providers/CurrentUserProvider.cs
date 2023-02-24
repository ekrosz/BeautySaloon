using BeautySaloon.DAL.Providers;
using System.Security.Claims;

namespace BeautySaloon.WebApi.Providers;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
        => Guid.TryParse(_httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimsIdentity.DefaultNameClaimType, StringComparison.OrdinalIgnoreCase))?.Value, out var userId)
        ? userId
        : throw new ArgumentNullException("UserId", "Не найден идентификатор в токене авторизации.");
}
