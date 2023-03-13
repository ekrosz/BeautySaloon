using BeautySaloon.DAL.Entities.Enums;
using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Order;

public record PayOrderRequestDto
{
    public PaymentMethod PaymentMethod { get; init; }

    public string? Comment { get; init; }
}

public class PayOrderRequestValidator : AbstractValidator<PayOrderRequestDto>
{
    public PayOrderRequestValidator()
    {
        RuleFor(_ => _.PaymentMethod)
            .NotNull()
            .NotEmpty()
            .IsInEnum();

        RuleFor(_ => _.Comment)
            .MaximumLength(500)
            .When(_ => _.Comment is not null);
    }
}
