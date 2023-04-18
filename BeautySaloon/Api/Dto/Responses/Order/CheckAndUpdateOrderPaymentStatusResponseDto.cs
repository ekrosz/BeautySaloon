using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Api.Dto.Responses.Order;

public record CheckAndUpdateOrderPaymentStatusResponseDto
{
    public PaymentStatus PaymentStatus { get; init; }
}
