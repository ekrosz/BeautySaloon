using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Subscription;

public record CosmeticServiceRequestDto
{
    public Guid Id { get; init; }

    public int Count { get; init; }
}

public class CosmeticServiceRequestValidator : AbstractValidator<CosmeticServiceRequestDto>
{
    public CosmeticServiceRequestValidator()
    {
        RuleFor(_ => _.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(_ => _.Count)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1);
    }
}
