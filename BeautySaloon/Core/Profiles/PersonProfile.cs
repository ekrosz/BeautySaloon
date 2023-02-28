using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Person;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;
public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PageResponseDto<Person>, PageResponseDto<GetPersonListItemResponseDto>>()
            .ForMember(dest => dest.Items, cfg => cfg.MapFrom(src => src.Items))
            .ForMember(dest => dest.TotalCount, cfg => cfg.MapFrom(src => src.TotalCount));

        CreateMap<Person, GetPersonListItemResponseDto>();

        CreateMap<Person, GetPersonResponseDto>()
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.Orders.Select(x => x.PersonSubscriptions.GroupBy(_ => _.SubscriptionCosmeticService.Subscription).Select(_ => _.Key))));

        CreateMap<Subscription, GetPersonResponseDto.SubscriptionResponseDto>();
    }
}
