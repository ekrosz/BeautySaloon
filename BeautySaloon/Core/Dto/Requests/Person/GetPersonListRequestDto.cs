﻿using BeautySaloon.Core.Dto.Common;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;

namespace BeautySaloon.Core.Dto.Requests.Person;

public record GetPersonListRequestDto
{
    public string? SearchString { get; init; }

    public PageRequestDto Page { get; set; } = new PageRequestDto();
}

public class GetPersonListRequestValidator : AbstractValidator<GetPersonListRequestDto>
{
    public GetPersonListRequestValidator()
    {
        RuleFor(_ => _.Page)
            .NotNull()
            .NotEmpty()
            .SetValidator(new PageRequestValidator());
    }
}