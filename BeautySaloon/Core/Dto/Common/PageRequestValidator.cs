using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Common;
public class PageRequestValidator : AbstractValidator<PageRequestDto>
{
    public PageRequestValidator()
    {
        RuleFor(_ => _.PageNumber)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1);

        RuleFor(_ => _.PageSize)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(500);
    }
}
