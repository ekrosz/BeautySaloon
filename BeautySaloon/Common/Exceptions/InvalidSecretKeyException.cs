using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class InvalidSecretKeyException : BusinessExceptions
{
    public InvalidSecretKeyException()
        : base(HttpStatusCode.Conflict, "Неверный секретный ключ токена обновления.")
    {
    }
}
