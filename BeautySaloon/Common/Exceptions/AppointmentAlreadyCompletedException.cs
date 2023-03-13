using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class AppointmentAlreadyCompletedException : BusinessExceptions
{
    public AppointmentAlreadyCompletedException()
        : base(HttpStatusCode.Conflict, $"Данная дапись уже выполнена.")
    {
    }
}
