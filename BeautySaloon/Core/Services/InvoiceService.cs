using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Invoice;
using BeautySaloon.Api.Dto.Responses.Invoice;
using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Core.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IQueryRepository<Invoice> _invoiceQueryRepository;

        private readonly IQueryRepository<Material> _materialQueryRepository;

        private readonly IQueryRepository<User> _userQueryRepository;

        private readonly IWriteRepository<Invoice> _invoiceWriteRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public InvoiceService(
        IQueryRepository<Invoice> invoiceQueryRepository,
        IQueryRepository<Material> materialQueryRepository,
        IQueryRepository<User> userQueryRepository,
        IWriteRepository<Invoice> invoiceWriteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        {
            _invoiceQueryRepository = invoiceQueryRepository;
            _materialQueryRepository = materialQueryRepository;
            _userQueryRepository = userQueryRepository;
            _invoiceWriteRepository = invoiceWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateInvoiceAsync(CreateInvoiceRequestDto request, CancellationToken cancellationToken = default)
        {

            if (request.EmployeeId.HasValue)
            {
               var isExistEmployee = await _userQueryRepository.ExistAsync(
               x => request.EmployeeId == x.Id,
               cancellationToken);

                if (!isExistEmployee)
                {
                    throw new UserNotFoundException(request.EmployeeId.Value);
                }
            }
            
            var materials = await _materialQueryRepository.FindAsync(
                x => request.Materials.Select(s => s.Id).Contains(x.Id),
                cancellationToken);

            var notExistMaterials = request.Materials
                .ExceptBy(materials.Select(x => x.Id), x => x.Id);

            if (notExistMaterials.Any())
            {
                throw new MaterialNotFoundException(notExistMaterials.First().Id);
            }

            var isExistInvoice = await _invoiceQueryRepository.ExistAsync(
                x => x.InvoiceDate.Date > request.InvoiceDate.Date,
                cancellationToken);

            if (isExistInvoice)
            {
                throw new InvalidInvoiceDateException(request.InvoiceDate);
            }

            if (request.InvoiceType == InvoiceType.Extradition)
            {
                await ValidateMaterialCountAsync(
                request.InvoiceDate,
                request.Materials,
                materials,
                null,
                cancellationToken);
            }

            var entity = new Invoice(
                request.InvoiceType,
                request.InvoiceDate,
                request.EmployeeId,
                request.Comment);

            var invoiceMaterial = request.Materials
                .Select(x => new InvoiceMaterial(x.Id, x.Count, x.Cost))
                .ToArray();

            entity.AddMaterials(invoiceMaterial);

            await _invoiceWriteRepository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateInvoiceAsync(ByIdWithDataRequestDto<UpdateInvoiceRequestDto> request, CancellationToken cancellationToken = default)
        {
            var entity = await _invoiceWriteRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new InvoiceNotFoundException(request.Id);

            if (request.Data.EmployeeId.HasValue)
            {
                var isExistEmployee = await _userQueryRepository.ExistAsync(
                x => request.Data.EmployeeId == x.Id,
                cancellationToken);

                if (!isExistEmployee)
                {
                    throw new UserNotFoundException(request.Data.EmployeeId.Value);
                }
            }

            var materials = await _materialQueryRepository.FindAsync(
                x => request.Data.Materials.Select(s => s.Id).Contains(x.Id),
                cancellationToken);

            var notExistMaterials = request.Data.Materials
                .ExceptBy(materials.Select(x => x.Id), x => x.Id);

            if (notExistMaterials.Any())
            {
                throw new MaterialNotFoundException(notExistMaterials.First().Id);
            }

            var isExistInvoice = await _invoiceQueryRepository.ExistAsync(
                x => x.InvoiceDate.Date > request.Data.InvoiceDate.Date && x.Id != request.Id,
                cancellationToken);

            if (isExistInvoice)
            {
                throw new InvalidInvoiceDateException(request.Data.InvoiceDate);
            }

            if (request.Data.InvoiceType == InvoiceType.Extradition)
            {
                await ValidateMaterialCountAsync(
                request.Data.InvoiceDate,
                request.Data.Materials,
                materials,
                entity.Id,
                cancellationToken);
            }            

            entity.Update(
                request.Data.InvoiceType,
                request.Data.InvoiceDate,
                request.Data.EmployeeId,
                request.Data.Comment);

            var invoiceMaterial = request.Data.Materials
                .Select(x => new InvoiceMaterial(x.Id, x.Count, x.Cost))
                .ToArray();

            entity.AddMaterials(invoiceMaterial);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteInvoiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
        {
            var entity = await _invoiceWriteRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new InvoiceNotFoundException(request.Id);

            _invoiceWriteRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<GetInvoiceResponseDto> GetInvoiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
        {
            var invoice = await _invoiceQueryRepository.GetByIdAsync(request.Id, cancellationToken)
                ?? throw new InvoiceNotFoundException(request.Id);

            return _mapper.Map<GetInvoiceResponseDto>(invoice);
        }

        public async Task<PageResponseDto<GetInvoiceListItemResponseDto>> GetInvoiceListAsync(GetInvoiceListRequestDto request, CancellationToken cancellationToken = default)
        {
            var orders = await _invoiceQueryRepository.GetPageAsync(
                request: request.Page,
                predicate: x => (string.IsNullOrWhiteSpace(request.SearchString)
                            || x.InvoiceMaterials.Any(y => y.Material.Name.ToLower().Contains(request.SearchString.ToLower())))
                        && ((!request.StartCreatedOn.HasValue || x.InvoiceDate.Date >= request.StartCreatedOn.Value.Date)
                            && (!request.EndCreatedOn.HasValue || x.InvoiceDate.Date <= request.EndCreatedOn.Value.Date))
                        && x.InvoiceType == request.InvoiceType,
                sortProperty: x => x.InvoiceDate,
                asc: false,
                cancellationToken);

            return _mapper.Map<PageResponseDto<GetInvoiceListItemResponseDto>>(orders);
        }

        private async Task ValidateMaterialCountAsync(
            DateTime invoiceDate,
            IEnumerable<MaterialRequestDto> requestMaterials,
            IEnumerable<Material> materials,
            Guid? invoiceId,
            CancellationToken cancellationToken = default)
        {
            foreach (var material in materials)
            {
                var receiptCount = await _invoiceQueryRepository.SumAsync(
                    x => (!invoiceId.HasValue || invoiceId.Value == x.Id)
                        && x.InvoiceType == InvoiceType.Receipt
                        && x.InvoiceDate.Date <= invoiceDate.Date,
                    x => x.InvoiceMaterials.Where(y => y.MaterialId == material.Id).Sum(y => y.Count),
                    cancellationToken);

                var extraditionCount = await _invoiceQueryRepository.SumAsync(
                    x => (!invoiceId.HasValue || invoiceId.Value == x.Id)
                        && x.InvoiceType == InvoiceType.Extradition
                        && x.InvoiceDate.Date <= invoiceDate.Date,
                    x => x.InvoiceMaterials.Where(y => y.MaterialId == material.Id).Sum(y => y.Count),
                    cancellationToken);

                var materialCount = requestMaterials.First(x => x.Id == material.Id).Count;

                if (receiptCount - extraditionCount - materialCount < 0)
                {
                    throw new NotEnoughMaterialException(material.Name, receiptCount - extraditionCount);
                }
            }
        }
        
    }
}
