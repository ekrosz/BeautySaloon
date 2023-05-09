using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Invoice;
using BeautySaloon.Api.Dto.Responses.Invoice;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Services.Contracts;
public interface IInvoiceService
{
    Task CreateInvoiceAsync(CreateInvoiceRequestDto request, CancellationToken cancellationToken = default);

    Task UpdateInvoiceAsync(ByIdWithDataRequestDto<UpdateInvoiceRequestDto> request, CancellationToken cancellationToken = default);

    Task DeleteInvoiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<GetInvoiceResponseDto> GetInvoiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<PageResponseDto<GetInvoiceListItemResponseDto>> GetInvoiceListAsync(GetInvoiceListRequestDto request, CancellationToken cancellationToken = default);
}

