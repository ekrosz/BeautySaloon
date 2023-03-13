using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.CosmeticService;
public record UpdateCosmeticServiceRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int ExecuteTimeInMinutes { get; init; }
}

public class UpdateCosmeticServiceRequestValidator : AbstractValidator<UpdateCosmeticServiceRequestDto>
{
    public UpdateCosmeticServiceRequestValidator()
    {
        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(_ => _.Description)
            .NotNull()
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(_ => _.ExecuteTimeInMinutes)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1440);
    }
}
