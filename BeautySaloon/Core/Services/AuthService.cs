using AutoMapper;
using BeautySaloon.Common.Utils;
using BeautySaloon.Core.Dto.Requests.Auth;
using BeautySaloon.Core.Dto.Responses.Auth;
using BeautySaloon.Core.Exceptions;
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

    private readonly IWriteRepository<User> _userRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly AuthenticationSettings _authenticationSettings;

    public AuthService(
        IWriteRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IOptionsSnapshot<AuthenticationSettings> authenticationSettings)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _authenticationSettings = authenticationSettings.Value;
    }

    public async Task<AuthorizeResponseDto> AuthorizeByCredentialsAsync(AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetFirstAsync(x => x.Login == request.Login)
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

        var user = await _userRepository.GetByIdAsync(refreshData.UserId)
            ?? throw new EntityNotFoundException($"Пользователь {refreshData.UserId} не найден.", typeof(User));

        if (!user.IsValidRefreshSecret(refreshData.RefreshSecretKey))
        {
            throw new InvalidSecretKeyException();
        }

        var accessToken = GenerateAccessToken(user);
        var refreshToken = await GenerateRefreshTokenAsync(user, cancellationToken);

        return new AuthorizeResponseDto(accessToken, refreshToken);
    }

    public async Task CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetFirstAsync(x => x.Login.Equals(request.Login));

        if (user is not null)
        {
            throw new EntityAlreadyExistException($"Пользователь {user.Login} уже существуюет", typeof(User));
        }

        var entity = new User(
            request.Role,
            request.Login,
            request.Password,
            request.Name,
            request.PhoneNumber,
            request.Email);

        await _userRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(UpdateUserByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(request.Id)
            ?? throw new EntityNotFoundException($"Пользователь {request.Id} не найден.", typeof(User));

        var isExistLogin = await _userRepository.ExistAsync(x => x.Login.Equals(request.Data.Login) && x.Id != request.Id);

        if (isExistLogin)
            throw new EntityAlreadyExistException($"Пользователь {request.Data.Login} уже существуюет", typeof(User));

        user.Update(
            request.Data.Role,
            request.Data.Login,
            request.Data.Password,
            request.Data.Name,
            request.Data.PhoneNumber,
            request.Data.Email);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
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

        return Convert.ToBase64String(CryptoUtility.EncryptData(refreshData, _authenticationSettings.RefreshSecurityBytesKey));
    }


    private RefreshDataDto ExtractRefreshData(string refreshToken)
    {
        var encryptedData = Convert.FromBase64String(refreshToken);

        return CryptoUtility.DecryptData<RefreshDataDto>(encryptedData, _authenticationSettings.RefreshSecurityBytesKey);
    }

    private struct RefreshDataDto
    {
        public Guid UserId { get; init; }

        public Guid RefreshSecretKey { get; init; }

        public long ExpiredOn { get; init; }
    }
}
