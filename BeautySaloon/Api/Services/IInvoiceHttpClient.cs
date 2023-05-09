using BeautySaloon.Api.Dto.Requests.Invoice;
using BeautySaloon.Api.Dto.Responses.Invoice;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IInvoiceHttpClient
{
    [Post("/api/invoices")]
    Task CreateAsync([Header("Authorization")] string accessToken, [Body] CreateInvoiceRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/invoices/{id}")]
    Task UpdateAsync([Header("Authorization")] string accessToken, Guid id, [Body] UpdateInvoiceRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/invoices/{id}")]
    Task DeleteAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/invoices/{id}")]
    Task<GetInvoiceResponseDto> GetAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/invoices")]
    Task<PageResponseDto<GetInvoiceListItemResponseDto>> GetListAsync([Header("Authorization")] string accessToken, [Query] GetInvoiceListRequestDto request, CancellationToken cancellationToken = default);
}
