namespace BeautySaloon.Common.Exceptions;

public class UserAlreadyExistException : EntityAlreadyExistException
{
    public UserAlreadyExistException(string propertyName, object value)
        : base("Данный пользователь уже существует.", propertyName, value)
    {
    }
}
