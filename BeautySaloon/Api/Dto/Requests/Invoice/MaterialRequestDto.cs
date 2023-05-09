using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Invoice;

public record MaterialRequestDto
{
    public Guid Id { get; init; }

    public int Count { get; init; }

    public decimal? Cost { get; init; }
}

public class MaterialRequestValidator : AbstractValidator<MaterialRequestDto>
{
    public MaterialRequestValidator()
    {
        RuleFor(_ => _.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(_ => _.Count)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1);

        RuleFor(_ => _.Cost)
            .GreaterThanOrEqualTo(0)
            .When(_ => _.Cost.HasValue);
    }
}
