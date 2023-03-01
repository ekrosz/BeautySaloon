using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Order;

public record CreateOrderRequestDto
{
    public Guid PersonId { get; set; }

    public IReadOnlyCollection<Guid> SubscriptionIds { get; set; } = Array.Empty<Guid>();
}

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequestDto>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(_ => _.PersonId)
            .NotNull()
            .NotEmpty();

        RuleFor(_ => _.SubscriptionIds)
            .NotNull()
            .NotEmpty();
    }
}
