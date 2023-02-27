using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Subscription;

public record UpdateSubscriptionRequestDto
{
    public string Name { get; init; } = string.Empty;

    public int? LifeTime { get; init; }

    public decimal Price { get; init; }

    public IReadOnlyCollection<CosmeticServiceRequestDto> CosmeticServices { get; init; } = Array.Empty<CosmeticServiceRequestDto>();
}

public class UpdateSubscriptionRequestValidator : AbstractValidator<CreateSubscriptionRequestDto>
{
    public UpdateSubscriptionRequestValidator()
    {
        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(_ => _.LifeTime)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(366)
            .When(_ => _.LifeTime.HasValue);

        RuleFor(_ => _.Price)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0);

        RuleForEach(_ => _.CosmeticServices)
            .NotNull()
            .SetValidator(new CosmeticServiceRequestValidator());
    }
}
