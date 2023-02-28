namespace BeautySaloon.Common.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message, Type entityType)
        : base($"Запрашиваемая запись типа {entityType.Name} не найдена: {message}")
    {
    }
}
