using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Common.Utils;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;
using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.User;
public class UpdateUserRequestDto
{
    public Role Role { get; set; }

    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public FullName Name { get; set; } = FullName.Empty;
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
