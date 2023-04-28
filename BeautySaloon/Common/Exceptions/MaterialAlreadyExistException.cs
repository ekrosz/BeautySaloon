namespace BeautySaloon.Common.Exceptions;

public class MaterialAlreadyExistException : EntityAlreadyExistException
{
    public MaterialAlreadyExistException(string propertyName, object value)
        : base("Данный материал уже существует.", propertyName, value)
    {
    }
}
