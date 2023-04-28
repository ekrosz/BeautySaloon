using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Material;

public record CreateMaterialRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}

public class CreateMaterialRequestValidator : AbstractValidator<CreateMaterialRequestDto>
{
    public CreateMaterialRequestValidator()
    {
        RuleFor(_ => _.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(_ => _.Description)
            .NotEmpty()
            .MaximumLength(500)
            .When(_ => _.Description is not null);
    }
}
