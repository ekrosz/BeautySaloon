using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Auth;

public record AuthorizeByRefreshTokenRequestDto
{
    public string RefreshToken { get; init; } = string.Empty;
}

public class AuthorizeByRefreshTokenRequestValidator : AbstractValidator<AuthorizeByRefreshTokenRequestDto>
{
    public AuthorizeByRefreshTokenRequestValidator()
    {
        RuleFor(_ => _.RefreshToken)
            .NotNull()
            .NotEmpty();
    }
}
