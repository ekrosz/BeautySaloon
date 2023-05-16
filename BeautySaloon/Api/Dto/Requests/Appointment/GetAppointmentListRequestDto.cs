namespace BeautySaloon.Api.Dto.Requests.Appointment;

public record GetAppointmentListRequestDto
{
    public string? SearchString { get; init; }
}
