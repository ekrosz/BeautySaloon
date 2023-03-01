using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Order;

public record CancelOrderRequestDto
{
    public string? Comment { get; init; }
}

public class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequestDto>
{
    public CancelOrderRequestValidator()
    {
        RuleFor(_ => _.Comment)
            .MinimumLength(500)
            .When(_ => _.Comment is not null);
    }
}
