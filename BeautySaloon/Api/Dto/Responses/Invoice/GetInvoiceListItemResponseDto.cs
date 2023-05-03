using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Api.Dto.Responses.Invoice;
public record GetInvoiceListItemResponseDto
{
    public Guid Id { get; init; }

    public InvoiceType InvoiceType { get; init; }

    public string? Comment { get; init; }

    public ModifierResponseDto Modifier { get; set; } = new();

    public FullName EmployeeName { get; init; } = FullName.Empty;

    public decimal? Cost { get; set; }
}
