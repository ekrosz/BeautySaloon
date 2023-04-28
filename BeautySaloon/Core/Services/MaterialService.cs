using AutoMapper;
using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Dto.Responses.Material;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.Core.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IQueryRepository<Material> _materialQueryRepository;

        private readonly IWriteRepository<Material> _materialWriteRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public MaterialService(
            IQueryRepository<Material> materialQueryRepository,
            IWriteRepository<Material> materialWriteRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _materialQueryRepository = materialQueryRepository;
            _materialWriteRepository = materialWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateMaterialAsync(CreateMaterialRequestDto request, CancellationToken cancellationToken = default)
        {
            var isExistName = await _materialQueryRepository.ExistAsync(
                x => x.Name.ToLower().Equals(request.Name.ToLower()),
                cancellationToken);

            if (isExistName)
            {
                throw new MaterialAlreadyExistException(nameof(request.Name), request.Name);
            }

            var entity = new Material(
                request.Name,
                request.Description);

            await _materialWriteRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteMaterialAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
        {
            var entity = await _materialWriteRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new MaterialNotFoundException(request.Id);

            _materialWriteRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<GetMaterialResponseDto> GetMaterialAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
        {
            var material = await _materialQueryRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new MaterialNotFoundException(request.Id);

            return _mapper.Map<GetMaterialResponseDto>(material);
        }
        public async Task<PageResponseDto<GetMaterialResponseDto>> GetMaterialListAsync(GetMaterialListRequestDto request, CancellationToken cancellationToken = default)
        {
            var materials = await _materialQueryRepository.GetPageAsync(
                request: request.Page,
                x => string.IsNullOrWhiteSpace(request.SearchString)
                    || x.Name.ToLower().Contains(request.SearchString.ToLower()),
                sortProperty: x => x.Name,
                cancellationToken: cancellationToken);

            return _mapper.Map<PageResponseDto<GetMaterialResponseDto>>(materials);
        }
        public async Task UpdateMaterialAsync(ByIdWithDataRequestDto<UpdateMaterialRequestDto> request, CancellationToken cancellationToken = default)
        {
            var isExistName = await _materialQueryRepository.ExistAsync(
                x => x.Name.ToLower().Equals(request.Data.Name.ToLower()) && x.Id != request.Id,
                cancellationToken);

            if (isExistName)
            {
                throw new MaterialAlreadyExistException(nameof(request.Data.Name), request.Data.Name);
            }

            var entity = await _materialWriteRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new MaterialNotFoundException(request.Id);

            entity.Update(
                request.Data.Name,
                request.Data.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
