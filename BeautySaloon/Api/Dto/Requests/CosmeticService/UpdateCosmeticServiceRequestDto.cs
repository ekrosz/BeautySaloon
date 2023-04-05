using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.CosmeticService;
public record UpdateCosmeticServiceRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int? ExecuteTimeInMinutes { get; init; }
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
            .NotEmpty()
            .MaximumLength(500)
            .When(_ => _.Description is not null);

        RuleFor(_ => _.ExecuteTimeInMinutes)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(1440)
            .When(x => x.ExecuteTimeInMinutes.HasValue);
    }
}
