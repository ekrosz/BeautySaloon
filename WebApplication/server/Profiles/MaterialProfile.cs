using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Dto.Responses.Material;
using WebApplication.Pages;

namespace WebApplication.Profiles
{
    public class MaterialProfile : Profile
    {
        public MaterialProfile()
        {
            CreateMap<GetMaterialResponseDto, EditMaterialComponent.MaterialRequest>();

            CreateMap<AddMaterialComponent.MaterialRequest, CreateMaterialRequestDto>();

            CreateMap<EditMaterialComponent.MaterialRequest, UpdateMaterialRequestDto>();
        }
    }
}
