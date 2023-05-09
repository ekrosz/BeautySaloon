using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Dto.Responses.Material;
using BeautySaloon.Common;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers
{
    [Route("api/materials")]
    [ApiController]

    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        private readonly IValidator<CreateMaterialRequestDto> _createMaterialRequestValidator;
        private readonly IValidator<UpdateMaterialRequestDto> _updateMaterialRequestValidator;
        private readonly IValidator<GetMaterialListRequestDto> _getMaterialListRequestValidator;
        private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;
        public MaterialsController(
         IMaterialService materialServiceService,
         IValidator<CreateMaterialRequestDto> createMaterialRequestValidator,
         IValidator<UpdateMaterialRequestDto> updateMaterialRequestValidator,
         IValidator<GetMaterialListRequestDto> getMaterialListRequestValidator,
         IValidator<ByIdRequestDto> byIdRequestValidator)
        {
            _materialService = materialServiceService;
            _createMaterialRequestValidator = createMaterialRequestValidator;
            _updateMaterialRequestValidator = updateMaterialRequestValidator;
            _getMaterialListRequestValidator = getMaterialListRequestValidator;
            _byIdRequestValidator = byIdRequestValidator;
        }
        [HttpGet]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<PageResponseDto<GetMaterialResponseDto>> GetListAsync([FromQuery] GetMaterialListRequestDto request, CancellationToken cancellationToken = default)
        {
            await _getMaterialListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

            return await _materialService.GetMaterialListAsync(request, cancellationToken);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<GetMaterialResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var requestById = new ByIdRequestDto(id);

            await _byIdRequestValidator.ValidateAndThrowAsync(requestById);

            return await _materialService.GetMaterialAsync(requestById, cancellationToken);
        }

        [HttpPost]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task CreateAsync([FromBody] CreateMaterialRequestDto request, CancellationToken cancellationToken = default)
        {
            await _createMaterialRequestValidator.ValidateAndThrowAsync(request);

            await _materialService.CreateMaterialAsync(request, cancellationToken);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateMaterialRequestDto request, CancellationToken cancellationToken = default)
        {
            await _updateMaterialRequestValidator.ValidateAndThrowAsync(request);

            var requestById = new ByIdWithDataRequestDto<UpdateMaterialRequestDto>(id, request);

            await _materialService.UpdateMaterialAsync(requestById, cancellationToken);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var requestById = new ByIdRequestDto(id);

            await _byIdRequestValidator.ValidateAndThrowAsync(requestById);

            await _materialService.DeleteMaterialAsync(requestById, cancellationToken);
        }
    }
}
