using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class AppointmentAlreadyCancelledException : BusinessExceptions
{
    public AppointmentAlreadyCancelledException()
        : base(HttpStatusCode.Conflict, $"Данная дапись уже отменена.")
    {
    }
}
