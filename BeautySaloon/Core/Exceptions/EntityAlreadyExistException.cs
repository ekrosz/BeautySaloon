namespace BeautySaloon.Core.Exceptions;

public class EntityAlreadyExistException : Exception
{
    public EntityAlreadyExistException(string message, Type entityType)
        : base($"Запись типа {entityType.Name} уже существует: {message}")
    {
    }
}
