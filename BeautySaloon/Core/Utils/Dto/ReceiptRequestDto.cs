namespace BeautySaloon.Core.Utils.Dto;

public record ReceiptRequestDto
{
    public int Number { get; init; }

    public DateTime PaidOn { get; init; }

    public decimal Cost { get; init; }

    public string PersonFullName { get; init; } = string.Empty;

    public string PersonPhoneNumber { get; init; } = string.Empty;

    public IReadOnlyCollection<ReceiptItem> Items { get; init; } = Array.Empty<ReceiptItem>();

    public record ReceiptItem
    {
        public string Name { get; init; } = string.Empty;

        public int Count { get; init; }

        public decimal Price { get; init; }

        public decimal TotalPrice { get; init; }
    }
}
