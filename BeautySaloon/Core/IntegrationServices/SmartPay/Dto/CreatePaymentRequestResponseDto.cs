namespace BeautySaloon.Core.IntegrationServices.SmartPay.Dto;

public record CreatePaymentRequestResponseDto
{
    public string InvoiceId { get; init; } = default!;

    public byte[] QrCode { get; init; } = default!;
}
