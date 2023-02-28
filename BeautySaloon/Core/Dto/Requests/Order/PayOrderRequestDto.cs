using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Core.Dto.Requests.Order;

public record PayOrderRequestDto
{
    public PaymentMethod PaymentMethod { get; set; }
}
