using AutoMapper;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.Api.Dto.Responses.Material;

namespace BeautySaloon.Core.Profiles;

public class MaterialProfile : Profile
{
    public MaterialProfile()
    {
        CreateMap<PageResponseDto<Material>, PageResponseDto<GetMaterialResponseDto>>();

        CreateMap<Material, GetMaterialResponseDto>();
    }
}
