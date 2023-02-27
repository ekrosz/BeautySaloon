using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Subscription;
using BeautySaloon.Core.Dto.Responses.Subscription;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Services.Contracts;

public interface ISubscriptionService
{
    Task CreateSubscriptionAsync(CreateSubscriptionRequestDto request, CancellationToken cancellationToken = default);

    Task UpdateSubscriptionAsync(ByIdWithDataRequestDto<UpdateSubscriptionRequestDto> request, CancellationToken cancellationToken = default);

    Task DeleteSubscriptionAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<GetSubscriptionResponseDto> GetSubscriptionAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<PageResponseDto<GetSubscriptionListItemResponseDto>> GetSubscriptionListAsync(GetSubscriptionListRequestDto request, CancellationToken cancellationToken = default);
}
