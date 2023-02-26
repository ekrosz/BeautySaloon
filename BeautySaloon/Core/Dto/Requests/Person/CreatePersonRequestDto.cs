﻿using BeautySaloon.Common.Utils;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Person;

public record CreatePersonRequestDto
{
    public FullName Name { get; init; } = FullName.Empty;

    public DateTime BirthDate { get; init; }

    public string? Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string? Comment { get; init; }
}

public class CreatePersonRequestValidator : AbstractValidator<CreatePersonRequestDto>
{
    public CreatePersonRequestValidator()
    {
        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .SetValidator(new FullNameValidator());

        RuleFor(_ => _.BirthDate)
            .NotNull()
            .NotEmpty()
            .GreaterThan(new DateTime(1900, 1, 1))
            .LessThan(DateTime.UtcNow);

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

        RuleFor(_ => _.Comment)
            .MaximumLength(500)
            .When(_ => _.Comment is not null);
    }
}