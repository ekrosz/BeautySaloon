using BeautySaloon.Common;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Person;
using BeautySaloon.Core.Dto.Responses.Person;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Authorize(Roles = Constants.Roles.AdminAndEmployee)]
[Route("api/persons")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _personService;

    private readonly IValidator<CreatePersonRequestDto> _createPersonRequestValidator;
    private readonly IValidator<UpdatePersonRequestDto> _updatePersonRequestValidator;
    private readonly IValidator<AddSubscriptionRequestDto> _addSubscriptionRequestValidator;
    private readonly IValidator<GetPersonListRequestDto> _getPersonListRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public PersonsController(
        IPersonService personService,
        IValidator<CreatePersonRequestDto> createPersonRequestValidator,
        IValidator<UpdatePersonRequestDto> updatePersonRequestValidator,
        IValidator<AddSubscriptionRequestDto> addSubscriptionRequestValidator,
        IValidator<GetPersonListRequestDto> getPersonListRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _personService = personService;
        _createPersonRequestValidator = createPersonRequestValidator;
        _updatePersonRequestValidator = updatePersonRequestValidator;
        _addSubscriptionRequestValidator = addSubscriptionRequestValidator;
        _getPersonListRequestValidator = getPersonListRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] CreatePersonRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createPersonRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _personService.CreatePersonAsync(request, cancellationToken);
    }

    [HttpPost("{id}/subsciptions")]
    public async Task AddSubsriptionAsync([FromRoute] Guid id, [FromBody] AddSubscriptionRequestDto request, CancellationToken cancellationToken = default)
    {
        await _addSubscriptionRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<AddSubscriptionRequestDto>(id, request);

        await _personService.AddSubscriptionAsync(requestById, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdatePersonRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updatePersonRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdatePersonRequestDto>(id, request);

        await _personService.UpdatePersonAsync(requestById, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        await _personService.DeletePersonAsync(requestById, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<GetPersonResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _personService.GetPersonAsync(requestById, cancellationToken);
    }

    [HttpGet]
    public async Task<PageResponseDto<GetPersonListItemResponseDto>> GetListAsync([FromQuery] GetPersonListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getPersonListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _personService.GetPersonListAsync(request, cancellationToken);
    }
}
