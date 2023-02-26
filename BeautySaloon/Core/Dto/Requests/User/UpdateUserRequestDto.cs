using BeautySaloon.Common.Utils;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.User;
public class UpdateUserRequestDto
{
    public Role Role { get; init; }

    public string Login { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string? Email { get; init; } = string.Empty;

    public FullName Name { get; init; } = FullName.Empty;
}

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(_ => _.Role)
            .NotNull()
            .NotEmpty()
            .IsInEnum();

        RuleFor(_ => _.Login)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(_ => _.Password)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(_ => _.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .MaximumLength(12)
            .Must(_ => PhoneNumberUtilities.IsValid(_));

        RuleFor(_ => _.Email)
            .NotEmpty()
            .MaximumLength(255)
            .EmailAddress()
            .When(_ => _.Email is not null);

        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .SetValidator(new FullNameValidator());
    }
}
