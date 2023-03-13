namespace BeautySaloon.Common.Exceptions;

public class CosmeticServiceAlreadyExistException : EntityAlreadyExistException
{
    public CosmeticServiceAlreadyExistException(string propertyName, object value)
        : base("Данная косметическая услуга уже существует.", propertyName, value)
    {
    }
}
