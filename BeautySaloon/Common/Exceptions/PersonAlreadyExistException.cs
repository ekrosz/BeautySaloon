namespace BeautySaloon.Common.Exceptions;

public class PersonAlreadyExistException : EntityAlreadyExistException
{
    public PersonAlreadyExistException(string propertyName, object value)
        : base("Данный клиент уже существует.", propertyName, value)
    {
    }
}
