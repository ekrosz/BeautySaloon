using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Dto.Responses.Order;

public record GetOrderResponseDto
{
    public Guid Id { get; init; }

    public decimal Cost { get; init; }

    public PaymentStatus PaymentStatus { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public DateTime CreatedOn { get; init; }

    public DateTime UpdatedOn { get; init; }

    public IReadOnlyCollection<SubscriptionResponseDto> Subscriptions { get; init; } = Array.Empty<SubscriptionResponseDto>();

    public record SubscriptionResponseDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public int? LifeTime { get; init; }

        public decimal Price { get; init; }
    }

    public record PersonResponseDto
    {
        public Guid Id { get; init; }

        public FullName Name { get; init; } = FullName.Empty;

        public string PhoneNumber { get; init; } = string.Empty;
    }
}
