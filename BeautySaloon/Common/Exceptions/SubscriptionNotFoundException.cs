namespace BeautySaloon.Common.Exceptions;

public class SubscriptionNotFoundException : EntityNotFoundException
{
    public SubscriptionNotFoundException(Guid subscriptionId)
        : base("Запрашиваемый абонемент не найден.", subscriptionId)
    {
    }
}
