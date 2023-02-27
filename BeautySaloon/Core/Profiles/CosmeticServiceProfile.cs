using AutoMapper;
using BeautySaloon.Core.Dto.Responses.CosmeticService;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;

public class CosmeticServiceProfile : Profile
{
    public CosmeticServiceProfile()
    {
        CreateMap<PageResponseDto<CosmeticService>, PageResponseDto<GetCosmeticServiceResponseDto>>()
            .ForMember(dest => dest.Items, cfg => cfg.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalCount, cfg => cfg.MapFrom(src => src.TotalCount));

        CreateMap<CosmeticService, GetCosmeticServiceResponseDto>();
    }
}
