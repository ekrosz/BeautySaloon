using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class RefreshTokenExpiredException : BusinessExceptions
{
    public RefreshTokenExpiredException()
        : base(HttpStatusCode.Conflict, "Время жизни токена обновления истек.")
    {
    }
}
