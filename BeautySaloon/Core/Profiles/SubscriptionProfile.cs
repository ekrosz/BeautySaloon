using AutoMapper;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Entities;
using BeautySaloon.Api.Dto.Responses.Subscription;
using BeautySaloon.Api.Dto.Responses.Common;

namespace BeautySaloon.Core.Profiles;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<PageResponseDto<Subscription>, PageResponseDto<GetSubscriptionListItemResponseDto>>();

        CreateMap<Subscription, GetSubscriptionListItemResponseDto>();

        CreateMap<Subscription, GetSubscriptionResponseDto>()
            .ForMember(dest => dest.CosmeticServices, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServices));

        CreateMap<SubscriptionCosmeticService, CosmeticServiceResponseDto>()
            .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.CosmeticService.Id))
            .ForMember(dest => dest.Name, cfg => cfg.MapFrom(src => src.CosmeticService.Name))
            .ForMember(dest => dest.ExecuteTimeInMinutes, cfg => cfg.MapFrom(src => src.CosmeticService.ExecuteTimeInMinutes))
            .ForMember(dest => dest.Description, cfg => cfg.MapFrom(src => src.CosmeticService.Description))
            .ForMember(dest => dest.Count, cfg => cfg.MapFrom(src => src.Count));
    }
}
