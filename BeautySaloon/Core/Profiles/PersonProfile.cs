using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.Core.Dto.Responses.Person;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;

public class PersonProfile : Profile
{
    private static readonly IReadOnlyCollection<PersonSubscriptionStatus> AllowStatuses = new[]
    {
        PersonSubscriptionStatus.NotPaid,
        PersonSubscriptionStatus.Paid,
        PersonSubscriptionStatus.Completed
    };

    public PersonProfile()
    {
        CreateMap<PageResponseDto<Person>, PageResponseDto<GetPersonListItemResponseDto>>();

        CreateMap<Person, GetPersonListItemResponseDto>();

        CreateMap<Person, GetPersonResponseDto>()
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.Orders.SelectMany(
                x => x.PersonSubscriptions
                    .Where(_ => AllowStatuses.Contains(_.Status)))
                    .GroupBy(_ => _.SubscriptionCosmeticService.Subscription.Id)
                    .Select(_ => _.First().SubscriptionCosmeticService.Subscription)));

        CreateMap<Subscription, SubscriptionResponseDto>();
    }
}
