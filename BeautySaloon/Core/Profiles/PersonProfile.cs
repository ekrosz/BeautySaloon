using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Person;
using AutoMapper;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Entities.Enums;

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
                    .Where(x => x.Status == PersonSubscriptionStatus.Paid)
                    .GroupBy(_ => _.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                    .Select(_ => new SubscriptionResponseDto
                    {
                        Id = _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id,
                        Name = _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                        Price = _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price,
                        LifeTimeInDays = _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.LifeTimeInDays.HasValue
                            ? _.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.LifeTimeInDays - Convert.ToInt32((DateTime.UtcNow - x.UpdatedOn).TotalDays)
                            : null
                    }))));
    }
}
