using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BeautySaloon.Core.Dto.Responses.Order;

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

    public ModifierResponseDto Modifier { get; set; } = new();

    public IReadOnlyCollection<SubscriptionResponseDto> Subscriptions { get; init; } = Array.Empty<SubscriptionResponseDto>();
}
