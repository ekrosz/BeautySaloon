using BeautySaloon.Core.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Subscription;

public record GetSubscriptionListRequestDto
{
    public string SearchString { get; init; } = string.Empty;

    public PageRequestDto Page { get; init; } = new();
}

public class GetSubscriptionListRequestValidator : AbstractValidator<GetSubscriptionListRequestDto>
{
    public GetSubscriptionListRequestValidator()
    {
        RuleFor(_ => _.Page)
            .NotNull()
            .SetValidator(new PageRequestValidator());
    }
}
