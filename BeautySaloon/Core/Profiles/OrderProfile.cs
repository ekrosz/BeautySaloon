using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Order;
using AutoMapper;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<PageResponseDto<Order>, PageResponseDto<GetOrderResponseDto>>();

        CreateMap<Order, GetOrderResponseDto>()
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.PersonSubscriptions
                .GroupBy(_ => _.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                .Select(_ => _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot)))
            .ForMember(dest => dest.Modifier, cfg => cfg.MapFrom(src => src.Modifier));

        CreateMap<Person, PersonResponseDto>();

        CreateMap<SubscriptionSnapshot, SubscriptionResponseDto>();
    }
}
