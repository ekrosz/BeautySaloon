using AutoMapper;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Entities;
using BeautySaloon.Core.Dto.Responses.Subscription;

namespace BeautySaloon.Core.Profiles;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<PageResponseDto<Subscription>, PageResponseDto<GetSubscriptionListItemResponseDto>>();

        CreateMap<Subscription, GetSubscriptionListItemResponseDto>();

        CreateMap<Subscription, GetSubscriptionResponseDto>()
            .ForMember(dest => dest.CosmeticServices, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServices.Select(x => x.CosmeticService)));

        CreateMap<CosmeticService, GetSubscriptionResponseDto.CosmeticServiceResponseDto>();
    }
}
