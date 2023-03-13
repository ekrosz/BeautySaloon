using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class PersonSubscriptionAlreadyInProgressException : BusinessExceptions
{
    public PersonSubscriptionAlreadyInProgressException(string subscriptionName, string cosmeticServiceName)
        : base(HttpStatusCode.Conflict, $"Услуга {cosmeticServiceName} абонемента {subscriptionName} уже используется в другой записи клиента.")
    {
    }
}
