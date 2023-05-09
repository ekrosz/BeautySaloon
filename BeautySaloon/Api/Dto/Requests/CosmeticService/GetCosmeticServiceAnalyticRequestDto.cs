using BeautySaloon.Api.Dto.Requests.Subscription;
using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.CosmeticService;

public record GetCosmeticServiceAnalyticRequestDto
{
    public DateTime? StartCreatedOn { get; init; }

    public DateTime? EndCreatedOn { get; init; }
}

public class GetCosmeticServiceAnalyticRequestValidator : AbstractValidator<GetCosmeticServiceAnalyticRequestDto>
{
    public GetCosmeticServiceAnalyticRequestValidator()
    {
        RuleFor(x => x.StartCreatedOn)
            .NotEmpty()
            .When(x => x.StartCreatedOn.HasValue);

        RuleFor(x => x.EndCreatedOn)
            .NotEmpty()
            .When(x => x.EndCreatedOn.HasValue);

        RuleFor(x => x.StartCreatedOn)
            .LessThanOrEqualTo(x => x.EndCreatedOn)
            .When(x => x.StartCreatedOn.HasValue && x.EndCreatedOn.HasValue);
    }
}
