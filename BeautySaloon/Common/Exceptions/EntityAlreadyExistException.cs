using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class EntityAlreadyExistException : BusinessExceptions
{
    public EntityAlreadyExistException(string message, string propertyName, object value)
        : base(HttpStatusCode.Conflict, message)
    {
        Data.Add(propertyName, value);
    }
}
