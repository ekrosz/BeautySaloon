using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Core.Utils.Dto;

public record OrderReportRequestDto
{
    public DateTime StartCreatedOn { get; init; }

    public DateTime EndCreatedOn { get; init; }

    public decimal TotalCost { get; init; }

    public IReadOnlyCollection<OrderItem> Items { get; init; } = Array.Empty<OrderItem>();

    public record OrderItem
    {
        public string PersonFullName { get; init; } = string.Empty;

        public string EmployeeFullName { get; init; } = string.Empty;

        public string SubscriptionNames { get; init; } = string.Empty;

        public PaymentStatus PaymentStatus { get; init; }

        public decimal Cost { get; init; }

        public DateTime CreatedOn { get; init; }
    }
}
