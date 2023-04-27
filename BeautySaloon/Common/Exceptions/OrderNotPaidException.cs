using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class OrderNotPaidException : BusinessExceptions
{
    public OrderNotPaidException()
        : base(HttpStatusCode.Conflict, $"Данный заказ еще не оплачен.")
    {
    }
}
