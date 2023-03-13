using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class OrderAlreadyCancelledException : BusinessExceptions
{
    public OrderAlreadyCancelledException()
        : base(HttpStatusCode.Conflict, $"Данный заказ уже отменен.")
    {
    }
}
