using BeautySaloon.Api.Dto.Common;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.Api.Dto.Requests.Invoice;

public record GetInvoiceListRequestDto
{
    public InvoiceType? InvoiceType { get; init; }

    public string? SearchString { get; init; }

    public PageRequestDto Page { get; init; } = new();
}

public class GetInvoiceListRequestValidator : AbstractValidator<GetInvoiceListRequestDto>
{
    public GetInvoiceListRequestValidator()
    {
        RuleFor(_ => _.Page)
            .NotNull()
            .SetValidator(new PageRequestValidator());

        RuleFor(_ => _.InvoiceType)
            .IsInEnum()
            .When(_ => _.InvoiceType.HasValue);
    }
}
