using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class InvalidUserCredentialsException : BusinessExceptions
{
    public InvalidUserCredentialsException()
        : base(HttpStatusCode.Conflict, "Введен неверный логин и/или пароль.")
    {
    }
}
