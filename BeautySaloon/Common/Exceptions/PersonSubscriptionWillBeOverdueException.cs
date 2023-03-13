using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class PersonSubscriptionWillBeOverdueException : BusinessExceptions
{
    public PersonSubscriptionWillBeOverdueException(string subscriptionName, string cosmeticServiceName)
        : base(HttpStatusCode.Conflict, $"Услуга {cosmeticServiceName} абонемента {subscriptionName} уже просрочена или истечет до указанной даты.")
    {
    }
}
