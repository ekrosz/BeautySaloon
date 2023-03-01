using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.Core.Dto.Responses.Order;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<PageResponseDto<Order>, PageResponseDto<GetOrderResponseDto>>();

        CreateMap<Order, GetOrderResponseDto>()
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.PersonSubscriptions
                .GroupBy(_ => _.SubscriptionCosmeticService.Subscription.Id)
                .Select(_ => _.First().SubscriptionCosmeticService.Subscription)));

        CreateMap<Person, PersonResponseDto>();

        CreateMap<Subscription, SubscriptionResponseDto>();
    }
}
