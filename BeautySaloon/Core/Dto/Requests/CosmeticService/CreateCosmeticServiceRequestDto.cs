using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.CosmeticService;

public record CreateCosmeticServiceRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int ExecuteTime { get; init; }
}

public class CreateCosmeticServiceRequestValidator : AbstractValidator<CreateCosmeticServiceRequestDto>
{
    public CreateCosmeticServiceRequestValidator()
    {
        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(_ => _.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(_ => _.ExecuteTime)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1440);
    }
}
