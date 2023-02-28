namespace BeautySaloon.Common.Exceptions;

public class InvalidSecretKeyException : Exception
{
    public InvalidSecretKeyException()
        : base("Неверный ключ токена обновления.")
    {
    }
}
