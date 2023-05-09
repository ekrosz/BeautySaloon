using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Api.Dto.Responses.Invoice;
public record GetInvoiceListItemResponseDto
{
    public Guid Id { get; init; }

    public InvoiceType InvoiceType { get; init; }

    public DateTime InvoiceDate { get; init; }

    public string? Comment { get; init; }

    public UserResponseDto Modifier { get; init; } = new();

    public UserResponseDto? Employee { get; init; } = new();

    public decimal? Cost { get; init; }
}
