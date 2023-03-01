﻿using BeautySaloon.Core.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Order;

public class GetOrderListRequestDto
{
    public Guid PersonId { get; init; }

    public string? SearchString { get; init; } = string.Empty;

    public DateTime? StartCreatedOn { get; init; }

    public DateTime? EndCreatedOn { get; init; }

    public PageRequestDto Page { get; init; } = new();
}

public class GetOrderListRequestValidator : AbstractValidator<GetOrderListRequestDto>
{
    public GetOrderListRequestValidator()
    {
        RuleFor(_ => _.StartCreatedOn)
            .LessThanOrEqualTo(_ => _.EndCreatedOn)
            .When(_ => _.StartCreatedOn.HasValue && _.EndCreatedOn.HasValue);

        RuleFor(_ => _.EndCreatedOn)
            .GreaterThanOrEqualTo(_ => _.StartCreatedOn)
            .When(_ => _.StartCreatedOn.HasValue && _.EndCreatedOn.HasValue);

        RuleFor(_ => _.Page)
            .NotNull()
            .SetValidator(new PageRequestValidator());
    }
}