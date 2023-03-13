using BeautySaloon.Api.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.CosmeticService;

public record GetCosmeticServiceListRequestDto
{
    public string? SearchString { get; init; } = string.Empty;

    public PageRequestDto Page { get; init; } = new();
}

public class GetCosmeticServiceListRequestValidator : AbstractValidator<GetCosmeticServiceListRequestDto>
{
    public GetCosmeticServiceListRequestValidator()
    {
        RuleFor(_ => _.Page)
            .NotNull()
            .SetValidator(new PageRequestValidator());
    }
}
