using BeautySaloon.Api.Dto.Common;
using FluentValidation;
using Newtonsoft.Json;

namespace BeautySaloon.Api.Dto.Requests.Appointment;

public record CreateAppointmentRequestDto
{
    public Guid PersonId { get; init; }

    public DateTime AppointmentDate { get; init; }

    public string? Comment { get; init; }

    public IReadOnlyCollection<Guid> PersonSubcriptionIds { get; init; } = Array.Empty<Guid>();
}

public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequestDto>
{
    public CreateAppointmentRequestValidator()
    {
        RuleFor(_ => _.PersonId)
            .NotNull()
            .NotEmpty();

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
