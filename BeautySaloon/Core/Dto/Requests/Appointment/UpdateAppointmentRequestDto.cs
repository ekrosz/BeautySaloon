﻿using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Appointment;

public record UpdateAppointmentRequestDto
{
    public DateTime AppointmentDate { get; init; }

    public string? Comment { get; init; }

    public IReadOnlyCollection<Guid> PersonSubcriptionIds { get; init; } = Array.Empty<Guid>();
}

public class UpdateAppointmentRequestValidator : AbstractValidator<UpdateAppointmentRequestDto>
{
    public UpdateAppointmentRequestValidator()
    {
        RuleFor(_ => _.AppointmentDate)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.UtcNow);

        RuleFor(_ => _.Comment)
            .MaximumLength(500)
            .When(_ => _.Comment is not null);

        RuleForEach(_ => _.PersonSubcriptionIds)
            .NotNull()
            .NotEmpty();
    }
}
