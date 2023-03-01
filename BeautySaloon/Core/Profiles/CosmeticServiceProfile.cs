using AutoMapper;
using BeautySaloon.Core.Dto.Responses.CosmeticService;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;

public class CosmeticServiceProfile : Profile
{
    public CosmeticServiceProfile()
    {
        CreateMap<PageResponseDto<CosmeticService>, PageResponseDto<GetCosmeticServiceResponseDto>>();

        CreateMap<CosmeticService, GetCosmeticServiceResponseDto>();
    }
}
