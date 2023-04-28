using BeautySaloon.Api.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Material;

public record GetMaterialListRequestDto
{
    public string? SearchString { get; init; } = string.Empty;

    public PageRequestDto Page { get; init; } = new();
}

public class GetMaterialListRequestValidator : AbstractValidator<GetMaterialListRequestDto>
{
    public GetMaterialListRequestValidator()
    {
        RuleFor(_ => _.Page)
            .NotNull()
            .SetValidator(new PageRequestValidator());
    }
}
