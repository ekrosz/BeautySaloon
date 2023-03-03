namespace BeautySaloon.Common.Exceptions;

public class SubscriptionAlreadyExistException : EntityAlreadyExistException
{
    public SubscriptionAlreadyExistException(string propertyName, object value)
        : base("Данный абонемент уже существует.", propertyName, value)
    {
    }
}
