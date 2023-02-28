namespace BeautySaloon.Core.Dto.Requests.Order;

public record UpdateOrderRequestDto
{
    public IReadOnlyCollection<Guid> SubscriptionIds { get; set; } = Array.Empty<Guid>();
}
