using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Order;

public record CreateOrderRequestDto
{
    public Guid PersonId { get; init; }

    public string? Comment { get; init; }

    public IReadOnlyCollection<Guid> SubscriptionIds { get; init; } = Array.Empty<Guid>();
}

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(_ => _.PersonId)
            .NotNull()
            .NotEmpty();

        RuleFor(_ => _.Comment)
            .MaximumLength(500)
            .When(_ => _.Comment is not null);

        RuleFor(_ => _.SubscriptionIds)
            .NotNull()
            .NotEmpty();
    }
}
