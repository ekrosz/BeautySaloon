using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Person;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Entities;
using BeautySaloon.Core.Dto.Responses.Subscription;

namespace BeautySaloon.Core.Profiles;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<PageResponseDto<Subscription>, PageResponseDto<GetSubscriptionListItemResponseDto>>()
            .ForMember(dest => dest.Items, cfg => cfg.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalCount, cfg => cfg.MapFrom(src => src.TotalCount));

        CreateMap<Subscription, GetSubscriptionListItemResponseDto>();

        CreateMap<Subscription, GetSubscriptionResponseDto>()
            .ForMember(dest => dest.CosmeticServices, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServices.Select(x => x.CosmeticService)));

        CreateMap<CosmeticService, GetSubscriptionResponseDto.CosmeticServiceResponseDto>();
    }
}
