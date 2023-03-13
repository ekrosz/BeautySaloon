using FluentValidation;

namespace BeautySaloon.Api.Dto.Common;

public record ByIdRequestDto(Guid Id);

public class ByIdRequestValidator : AbstractValidator<ByIdRequestDto>
{
    public ByIdRequestValidator()
    {
        RuleFor(_ => _.Id)
            .NotNull()
            .NotEmpty();
    }
}
