namespace BeautySaloon.Common.Exceptions;

public class OrderAlreadyPaidException : Exception
{
    public OrderAlreadyPaidException(Guid orderId)
        : base($"Заказ {orderId} уже был оплачен.")
    {
    }
}
