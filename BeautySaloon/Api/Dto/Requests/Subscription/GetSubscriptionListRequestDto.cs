using BeautySaloon.Api.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Subscription;

public record GetSubscriptionListRequestDto
{
    public string? SearchString { get; init; }

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
