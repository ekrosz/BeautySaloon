using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class OrderAlreadyPaidException : BusinessExceptions
{
    public OrderAlreadyPaidException()
        : base(HttpStatusCode.Conflict, $"Данный заказ уже оплачен.")
    {
    }
}
