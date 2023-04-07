using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.Api.Dto.Responses.Order;
using WebApplication.Pages;

namespace WebApplication.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<AddOrderComponent.OrderRequest, CreateOrderRequestDto>();

        CreateMap<EditOrderComponent.OrderRequest, UpdateOrderRequestDto>();

        CreateMap<GetOrderResponseDto, EditOrderComponent.OrderRequest>()
            .ForMember(dest => dest.SubscriptionIds, cfg => cfg.MapFrom(src => src.Subscriptions.Select(x => x.Id)));
    }
}
