using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Order;

public record UpdateOrderRequestDto
{
    public string? Comment { get; init; }

    public IReadOnlyCollection<Guid> SubscriptionIds { get; init; } = Array.Empty<Guid>();
}

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequestDto>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(_ => _.Comment)
            .MaximumLength(500)
            .When(_ => _.Comment is not null);

        RuleFor(_ => _.SubscriptionIds)
            .NotNull()
            .NotEmpty();
    }
}
