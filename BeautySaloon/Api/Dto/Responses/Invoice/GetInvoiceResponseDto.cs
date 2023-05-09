using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Api.Dto.Responses.Invoice;
public record GetInvoiceResponseDto
{
    public Guid Id { get; init; }

    public InvoiceType InvoiceType { get; init; }

    public DateTime InvoiceDate { get; init; }

    public string? Comment { get; init; }

    public UserResponseDto? Employee { get; init; }

    public IReadOnlyCollection<MaterialResponseDto> Materials { get; init; } = Array.Empty<MaterialResponseDto>();
}

