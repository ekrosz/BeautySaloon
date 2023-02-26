using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Person;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.PersonSubscriptions.Select(x => x.Subscription)));

        CreateMap<Subscription, GetPersonResponseDto.SubscriptionResponseDto>();
    }
}
