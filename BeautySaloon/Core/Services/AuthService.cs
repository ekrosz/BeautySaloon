using BeautySaloon.Common.Exceptions;
using BeautySaloon.Common.Utils;
using BeautySaloon.Core.Dto.Requests.Auth;
using BeautySaloon.Core.Dto.Responses.Auth;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.Core.Settings;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BeautySaloon.Core.Services;

public class AuthService : IAuthService
{
    private const string AuthType = "Token";

    private readonly IWriteRepository<User> _userWriteRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly AuthenticationSettings _authenticationSettings;

    public AuthService(
        IWriteRepository<User> userWriteRepository,
        IUnitOfWork unitOfWork,
        IOptionsSnapshot<AuthenticationSettings> authenticationSettings)
    {
        _userWriteRepository = userWriteRepository;
        _unitOfWork = unitOfWork;
        _authenticationSettings = authenticationSettings.Value;
    }

    public async Task<AuthorizeResponseDto> AuthorizeByCredentialsAsync(AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userWriteRepository.GetFirstAsync(x => x.Login.Equals(request.Login), cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {request.Login} не найден.", typeof(User));

        if (!user.IsValidPassword(request.Password))
        {
            throw new InvalidUserCredentialsException();
        }

        var refreshToken = await GenerateRefreshTokenAsync(user, cancellationToken);
        var accessToken = GenerateAccessToken(user);

        return new AuthorizeResponseDto(accessToken, refreshToken);
    }

    public async Task<AuthorizeResponseDto> AuthorizeByRefreshTokenAsync(AuthorizeByRefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var refreshData = ExtractRefreshData(request.RefreshToken);

        if (new DateTime(refreshData.ExpiredOn, DateTimeKind.Utc) <= DateTime.UtcNow)
        {
            throw new RefreshTokenExpiredException();
        }

        var user = await _userWriteRepository.GetByIdAsync(refreshData.UserId, cancellationToken)
            ?? throw new EntityNotFoundException($"Пользователь {refreshData.UserId} не найден.", typeof(User));

        if (!user.IsValidRefreshSecret(refreshData.RefreshSecretKey))
        {
            throw new InvalidSecretKeyException();
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user, cancellationToken);

        return new AuthorizeResponseDto(accessToken, refreshToken);
    }

    private string GenerateAccessToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var jwtDescription = new SecurityTokenDescriptor
        {
            Audience = _authenticationSettings.Audience,
            Issuer = _authenticationSettings.Issuer,
            Expires = DateTime.UtcNow.AddHours(_authenticationSettings.AccessTokenLifetime),
            SigningCredentials = new SigningCredentials(_authenticationSettings.AccessSecurityKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber)
            }, AuthType, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType)
        };

        return jwtTokenHandler.CreateEncodedJwt(jwtDescription);
    }

    private async Task<string> GenerateRefreshTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        var refreshSecret = user.GenerateNewRefreshSecret();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var refreshData = new RefreshDataDto
        {
            UserId = user.Id,
            RefreshSecretKey = refreshSecret,
            ExpiredOn = DateTime.UtcNow.AddHours(_authenticationSettings.RefreshTokenLifetime).Ticks
        };

        return Convert.ToBase64String(CryptoUtilities.EncryptData(refreshData, _authenticationSettings.RefreshSecurityBytesKey));
    }


    private RefreshDataDto ExtractRefreshData(string refreshToken)
    {
        var encryptedData = Convert.FromBase64String(refreshToken);

        return CryptoUtilities.DecryptData<RefreshDataDto>(encryptedData, _authenticationSettings.RefreshSecurityBytesKey);
    }

    private struct RefreshDataDto
    {
        public Guid UserId { get; init; }

        public Guid RefreshSecretKey { get; init; }

        public long ExpiredOn { get; init; }
    }
}
