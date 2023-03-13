using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class EntityNotFoundException : BusinessExceptions
{
    public EntityNotFoundException(string message, Guid entityId)
        : base(HttpStatusCode.NotFound, message)
    {
        Data.Add("EntityId", entityId);
    }
}
