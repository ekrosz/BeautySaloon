using BeautySaloon.Core.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Appointment;

public record GetAppointmentListRequestDto
{
    public string? SearchString { get; init; }

    public DateTime? StartAppointmentDate { get; init; }

    public DateTime? EndAppointmentDate { get; init; }

    public PageRequestDto Page { get; init; } = new();
}

public class GetAppointmentListRequestValidator : AbstractValidator<GetAppointmentListRequestDto>
{
    public GetAppointmentListRequestValidator()
    {
        RuleFor(_ => _.StartAppointmentDate)
            .LessThanOrEqualTo(_ => _.EndAppointmentDate)
            .When(_ => _.StartAppointmentDate.HasValue && _.EndAppointmentDate.HasValue);

        RuleFor(_ => _.EndAppointmentDate)
            .GreaterThanOrEqualTo(_ => _.StartAppointmentDate)
            .When(_ => _.StartAppointmentDate.HasValue && _.EndAppointmentDate.HasValue);

        RuleFor(_ => _.Page)
            .NotNull()
            .SetValidator(new PageRequestValidator());
    }
}
