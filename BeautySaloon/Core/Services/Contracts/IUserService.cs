using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.User;
using BeautySaloon.Api.Dto.Responses.User;

namespace BeautySaloon.Core.Services.Contracts;

public interface IUserService
{
    Task CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);

    Task UpdateUserAsync(ByIdWithDataRequestDto<UpdateUserRequestDto> request, CancellationToken cancellationToken = default);

    Task DeleteUserAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<GetUserResponseDto> GetUserAsync(ByIdRequestDto request, CancellationToken cancellationToken);

    Task<ItemListResponseDto<GetUserResponseDto>> GetUserListAsync(GetUserListRequestDto request, CancellationToken cancellationToken = default);
}
