using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Order;

public record UpdateOrderRequestDto
{
    public IReadOnlyCollection<Guid> SubscriptionIds { get; set; } = Array.Empty<Guid>();
}

public class UpdateOrderRequestValidator : AbstractValidator<UpdateOrderRequestDto>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(_ => _.SubscriptionIds)
            .NotNull()
            .NotEmpty();
    }
}
