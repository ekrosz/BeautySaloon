using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class PersonSubscriptionAlreadyCompletedException : BusinessExceptions
{
    public PersonSubscriptionAlreadyCompletedException(string subscriptionName, string cosmeticServiceName)
        : base(HttpStatusCode.Conflict, $"Услуга {cosmeticServiceName} абонемента {subscriptionName} уже была использована в другой записи клиента.")
    {
    }
}
