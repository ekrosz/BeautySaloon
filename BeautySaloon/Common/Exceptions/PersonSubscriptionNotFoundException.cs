namespace BeautySaloon.Common.Exceptions;

public class PersonSubscriptionNotFoundException : EntityNotFoundException
{
    public PersonSubscriptionNotFoundException(Guid personSubscriptionId)
        : base("Запрашиваемая услуга абонемента не найдена у клиента.", personSubscriptionId)
    {
    }
}
