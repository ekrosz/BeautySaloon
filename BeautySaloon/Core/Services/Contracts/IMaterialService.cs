using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Dto.Responses.Material;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.Core.Services.Contracts
{
    public interface IMaterialService
    {
        Task CreateMaterialAsync(CreateMaterialRequestDto request, CancellationToken cancellationToken = default);

        Task UpdateMaterialAsync(ByIdWithDataRequestDto<UpdateMaterialRequestDto> request, CancellationToken cancellationToken = default);

        Task DeleteMaterialAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

        Task<GetMaterialResponseDto> GetMaterialAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

        Task<PageResponseDto<GetMaterialResponseDto>> GetMaterialListAsync(GetMaterialListRequestDto request, CancellationToken cancellationToken = default);
    }
}
