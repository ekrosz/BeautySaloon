using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Person;
using BeautySaloon.Core.Dto.Responses.Person;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Services.Contracts;

public interface IPersonService
{
    Task CreatePersonAsync(CreatePersonRequestDto request, CancellationToken cancellationToken = default);

    Task UpdatePersonAsync(ByIdWithDataRequestDto<UpdatePersonRequestDto> request, CancellationToken cancellationToken = default);

    Task DeletePersonAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<GetPersonResponseDto> GetPersonAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<PageResponseDto<GetPersonListItemResponseDto>> GetPersonListAsync(GetPersonListRequestDto request, CancellationToken cancellationToken = default);
}