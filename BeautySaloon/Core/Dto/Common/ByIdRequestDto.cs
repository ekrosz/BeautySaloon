using FluentValidation;

namespace BeautySaloon.Core.Dto.Common;

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
