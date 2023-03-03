using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class PersonSubscriptionOverdueException : BusinessExceptions
{
    public PersonSubscriptionOverdueException(string subscriptionName, string cosmeticServiceName)
        : base(HttpStatusCode.Conflict, $"Услуга {cosmeticServiceName} абонемента {subscriptionName} уже просрочена.")
    {
    }
}
