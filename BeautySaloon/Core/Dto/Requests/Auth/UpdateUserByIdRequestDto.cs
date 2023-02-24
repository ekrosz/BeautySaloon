using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Auth;

public record UpdateUserByIdRequestDto(Guid Id, UpdateUserRequestDto Data);

public class UpdateUserByIdRequestValidator : AbstractValidator<UpdateUserByIdRequestDto>
{
    public UpdateUserByIdRequestValidator()
    {
        RuleFor(_ => _.Id)
            .NotNull()
            .NotEmpty();

        RuleFor(_ => _.Data)
            .SetValidator(new UpdateUserRequestValidator());
    }
}
