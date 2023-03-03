namespace BeautySaloon.Common.Exceptions;

public class AppointmentNotFoundException : EntityNotFoundException
{
    public AppointmentNotFoundException(Guid appointmentId)
        : base("Запрашиваемая запись не найдена.", appointmentId)
    {
    }
}
