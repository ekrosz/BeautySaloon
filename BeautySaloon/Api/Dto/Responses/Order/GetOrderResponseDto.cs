using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Api.Dto.Responses.Order;

public record GetOrderResponseDto
{
    public Guid Id { get; init; }

    public decimal Cost { get; init; }

    public PaymentStatus PaymentStatus { get; init; }

    public PaymentMethod PaymentMethod { get; init; }

    public DateTime CreatedOn { get; init; }

    public DateTime UpdatedOn { get; init; }

    public string? Comment { get; init; }

    public PersonResponseDto Person { get; init; } = new();

    public UserResponseDto Modifier { get; set; } = new();

    public IReadOnlyCollection<SubscriptionResponseDto> Subscriptions { get; init; } = Array.Empty<SubscriptionResponseDto>();
}
