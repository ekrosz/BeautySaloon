namespace BeautySaloon.Core.Dto.Requests.Order;

public record CreateOrderRequestDto
{
    public Guid PersonId { get; set; }

    public IReadOnlyCollection<Guid> SubscriptionIds { get; set; } = Array.Empty<Guid>();
}
