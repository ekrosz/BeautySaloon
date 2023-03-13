using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Person;
using AutoMapper;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PageResponseDto<Person>, PageResponseDto<GetPersonListItemResponseDto>>();

        CreateMap<Person, GetPersonListItemResponseDto>();

        CreateMap<Person, GetPersonResponseDto>()
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.Orders.SelectMany(
                x => x.PersonSubscriptions
                    .GroupBy(_ => _.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                    .Select(_ => _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot))));

        CreateMap<SubscriptionSnapshot, SubscriptionResponseDto>();
    }
}
