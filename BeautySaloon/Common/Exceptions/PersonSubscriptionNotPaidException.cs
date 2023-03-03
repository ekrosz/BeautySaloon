using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;
public class PersonSubscriptionNotPaidException : BusinessExceptions
{
    public PersonSubscriptionNotPaidException(string subscriptionName, string cosmeticServiceName)
        : base(HttpStatusCode.Conflict, $"Услуга {cosmeticServiceName} абонемента {subscriptionName} еще не оплачена.")
    {
    }
}
