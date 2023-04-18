namespace BeautySaloon.Core.IntegrationServices.SmartPay.Dto;

public record GetPaymentRequestResponseDto
{
    public PaymentRequestStatus PaymentStatus { get; init; }
}
