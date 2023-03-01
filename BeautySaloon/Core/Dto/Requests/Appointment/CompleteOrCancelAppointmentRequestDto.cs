using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Appointment;

public record CompleteOrCancelAppointmentRequestDto
{
    public string? Comment { get; init; }
}

public class CompleteOrCancelAppointmentRequestValidator : AbstractValidator<CompleteOrCancelAppointmentRequestDto>
{
    public CompleteOrCancelAppointmentRequestValidator()
    {
        RuleFor(_ => _.Comment)
            .MinimumLength(500)
            .When(_ => _.Comment is not null);
    }
}
