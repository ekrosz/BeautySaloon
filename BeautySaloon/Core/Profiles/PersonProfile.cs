using BeautySaloon.Api.Dto.Responses.Person;
using AutoMapper;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.Api.Dto.Responses.Common;

namespace BeautySaloon.Core.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<PageResponseDto<Person>, PageResponseDto<GetPersonListItemResponseDto>>();

        CreateMap<Person, GetPersonListItemResponseDto>();

        CreateMap<PersonSubscription, PersonSubscriptionCosmeticServiceResponseDto>()
            .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.Id))
            .ForMember(dest => dest.Subscription, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot))
            .ForMember(dest => dest.CosmeticService, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot));

        CreateMap<Person, GetPersonResponseDto>()
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.Orders
                .Where(x => !x.PersonSubscriptions.Any(_ => _.Status == PersonSubscriptionCosmeticServiceStatus.NotPaid || _.Status == PersonSubscriptionCosmeticServiceStatus.Cancelled))
                .SelectMany(x => x.PersonSubscriptions
                        .GroupBy(_ => _.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                        .Select(_ => new PersonSubscriptionResponseDto
                        {
                            Id = _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id,
                            Name = _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                            Price = _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price,
                            LifeTimeInDays = CalcLifeTimeInDays(_.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.LifeTimeInDays, x.UpdatedOn),
                            Status = CalcPersonSubscriptionStatus(_),
                            PaidOn = x.UpdatedOn
                        }))));
    }

    private int? CalcLifeTimeInDays(int? subscriptionLifeTime, DateTime orderUpdatedOn)
    {
        if (!subscriptionLifeTime.HasValue)
        {
            return null;
        }

        var currentLifeTime = subscriptionLifeTime.Value - Convert.ToInt32((DateTime.UtcNow - orderUpdatedOn).TotalDays);

        if (currentLifeTime > 0)
        {
            return currentLifeTime;
        }

        return 0;
    }

    private PersonSubscriptionStatus CalcPersonSubscriptionStatus(IEnumerable<PersonSubscription> personSubscriptions)
    {
        if (personSubscriptions.Any(x => x.Status == PersonSubscriptionCosmeticServiceStatus.Paid || x.Status == PersonSubscriptionCosmeticServiceStatus.InProgress))
        {
            return PersonSubscriptionStatus.Active;
        }

        if (personSubscriptions.All(x => x.Status == PersonSubscriptionCosmeticServiceStatus.Completed))
        {
            return PersonSubscriptionStatus.Completed;
        }

        if (personSubscriptions.Any(x => x.Status == PersonSubscriptionCosmeticServiceStatus.Overdue))
        {
            return PersonSubscriptionStatus.Overdue;
        }

        return PersonSubscriptionStatus.None;
    }
}
