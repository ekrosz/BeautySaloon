namespace BeautySaloon.Core.Exceptions;

public class RefreshTokenExpiredException : Exception
{
    public RefreshTokenExpiredException()
        : base("Время жизни токена обновления истек.")
    {
    }
}
