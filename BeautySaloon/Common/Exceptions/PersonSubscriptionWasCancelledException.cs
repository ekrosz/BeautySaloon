using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class PersonSubscriptionWasCancelledException : BusinessExceptions
{
    public PersonSubscriptionWasCancelledException(string subscriptionName, string cosmeticServiceName)
        : base(HttpStatusCode.Conflict, $"Услуга {cosmeticServiceName} абонемента {subscriptionName} была отменена.")
    {
    }
}