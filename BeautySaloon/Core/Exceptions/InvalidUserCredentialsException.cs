namespace BeautySaloon.Core.Exceptions;

public class InvalidUserCredentialsException : Exception
{
    public InvalidUserCredentialsException()
        : base("Введен неверный логин и/или пароль.")
    {
    }
}
