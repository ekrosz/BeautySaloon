using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Order;

public record GetOrderAnalyticRequestDto
{
    public int Year { get; init; }
}

public class GetOrderAnalyticRequestValidator : AbstractValidator<GetOrderAnalyticRequestDto>
{
    public GetOrderAnalyticRequestValidator()
    {
        RuleFor(x => x.Year)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(1900);
    }
}
