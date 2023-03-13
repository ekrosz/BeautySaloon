namespace BeautySaloon.Common.Exceptions;

public class OrderNotFoundException : EntityNotFoundException
{
    public OrderNotFoundException(Guid orderId)
        : base("Запрашиваемый заказ не найден.", orderId)
    {
    }
}
