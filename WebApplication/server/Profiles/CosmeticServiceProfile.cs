using AutoMapper;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using WebApplication.Pages;

namespace WebApplication.Profiles;

public class CosmeticServiceProfile : Profile
{
    public CosmeticServiceProfile()
    {
        CreateMap<GetCosmeticServiceResponseDto, EditCosmeticServiceComponent.CosmeticServiceRequest>();

        CreateMap<AddCosmeticServiceComponent.CosmeticServiceRequest, CreateCosmeticServiceRequestDto>();

        CreateMap<EditCosmeticServiceComponent.CosmeticServiceRequest, UpdateCosmeticServiceRequestDto>();
    }
}
