using BeautySaloon.DAL.Entities.ValueObjects;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Common;

public class FullNameValidator : AbstractValidator<FullName>
{
    public FullNameValidator()
    {
        RuleFor(_ => _.FirstName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(_ => _.LastName)
            .NotNull()
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(_ => _.MiddleName)
            .NotEmpty()
            .MaximumLength(50)
            .When(_ => _.MiddleName is not null);
    }
}
