using BeautySaloon.DAL.Entities.Enums;
using FluentValidation;

namespace BeautySaloon.Api.Dto.Requests.Invoice;
public record UpdateInvoiceRequestDto
{
    public InvoiceType InvoiceType { get; set; }

    public string? Comment { get; set; }

    public Guid? EmployeeId { get; set; }

    public IReadOnlyCollection<MaterialRequestDto> Materials { get; init; } = Array.Empty<MaterialRequestDto>();
}

public class UpdateInvoiceRequestValidator : AbstractValidator<UpdateInvoiceRequestDto>
{
    public UpdateInvoiceRequestValidator()
    {
        RuleFor(_ => _.InvoiceType)
            .NotNull()
            .IsInEnum();

        RuleFor(_ => _.Comment)
            .MaximumLength(500)
            .When(_ => !string.IsNullOrEmpty(_.Comment));

        RuleFor(_ => _.EmployeeId)
            .NotEmpty()
            .When(_ => _.EmployeeId.HasValue);

        RuleFor(_ => _.EmployeeId)
            .NotNull()
            .NotEmpty()
            .When(_ => _.InvoiceType == InvoiceType.Extradition);

        RuleFor(_ => _.Materials)
            .NotNull()
            .NotEmpty()
            .ChildRules(_ => _.RuleForEach(list => list)
                .NotNull()
                .SetValidator(new MaterialRequestValidator()));
    }
}
