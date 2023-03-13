using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class InvalidAppointmentDateException : BusinessExceptions
{
    public InvalidAppointmentDateException(DateTime startDateTime, DateTime endDateTime)
        : base(HttpStatusCode.Conflict, $"В период с {startDateTime:G} до {endDateTime:G} уже существует другая запись.")
    {
    }
}
