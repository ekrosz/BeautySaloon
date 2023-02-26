using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Person;

public record AddSubscriptionRequestDto
{
    public IReadOnlyCollection<Guid> SubscriptionIds { get; init; } = Array.Empty<Guid>();
}

public class AddSubscriptionRequestValidator : AbstractValidator<AddSubscriptionRequestDto>
{
    public AddSubscriptionRequestValidator()
    {
        RuleForEach(_ => _.SubscriptionIds)
            .NotNull()
            .NotEmpty();
    }
}
