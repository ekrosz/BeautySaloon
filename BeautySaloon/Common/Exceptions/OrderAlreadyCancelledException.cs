namespace BeautySaloon.Common.Exceptions;

public class OrderAlreadyCancelledException : Exception
{
    public OrderAlreadyCancelledException(Guid orderId)
        : base($"Заказ {orderId} уже отменен")
    {
    }
}
