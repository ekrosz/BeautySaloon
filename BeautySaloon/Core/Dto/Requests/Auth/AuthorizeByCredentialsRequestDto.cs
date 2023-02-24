using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Auth;

public record AuthorizeByCredentialsRequestDto
{
    public string Login { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}

public class AuthorizeByCredentialsRequestValidator : AbstractValidator<AuthorizeByCredentialsRequestDto>
{
    public AuthorizeByCredentialsRequestValidator()
    {
        RuleFor(_ => _.Login)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(_ => _.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);
    }
}
