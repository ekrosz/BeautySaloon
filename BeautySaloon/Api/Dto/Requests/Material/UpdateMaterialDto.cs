using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Material;
public record UpdateMaterialRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}

public class UpdateMaterialRequestValidator : AbstractValidator<UpdateMaterialRequestDto>
{
    public UpdateMaterialRequestValidator()
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
