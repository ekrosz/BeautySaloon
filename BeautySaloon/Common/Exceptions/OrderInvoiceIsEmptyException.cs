using BeautySaloon.Common.Exceptions.Abstract;
using System.Net;

namespace BeautySaloon.Common.Exceptions;

public class OrderInvoiceIsEmptyException : BusinessExceptions
{
    public OrderInvoiceIsEmptyException()
        : base(HttpStatusCode.Conflict, $"Для данного заказа еще не создан счет в банке.")
    {
    }
}
