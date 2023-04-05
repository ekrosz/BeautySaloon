using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.CosmeticService;

public record CreateCosmeticServiceRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; } = string.Empty;

    public int? ExecuteTimeInMinutes { get; init; }
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
            .NotEmpty()
            .MaximumLength(500)
            .When(_ => _.Description is not null);

        RuleFor(_ => _.ExecuteTimeInMinutes)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1440)
            .When(x => x.ExecuteTimeInMinutes.HasValue);
    }
}
