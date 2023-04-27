using AutoMapper;
using BeautySaloon.Core.Utils.Dto;
using BeautySaloon.DAL.Entities;

namespace BeautySaloon.Core.Utils.Profiles;

public class DocumentGeneratorProfile : Profile
{
    public DocumentGeneratorProfile()
    {
        CreateMap<Order, ReceiptRequestDto>()
            .ForMember(dest => dest.Number, cfg => cfg.MapFrom(src => src.Number))
            .ForMember(dest => dest.Cost, cfg => cfg.MapFrom(src => src.Cost))
            .ForMember(dest => dest.PaidOn, cfg => cfg.MapFrom(src => src.UpdatedOn))
            .ForMember(dest => dest.PersonFullName, cfg => cfg.MapFrom(src => src.Person.Name.ConcatedName))
            .ForMember(dest => dest.PersonPhoneNumber, cfg => cfg.MapFrom(src => src.Person.PhoneNumber))
            .ForMember(dest => dest.Items, cfg => cfg.MapFrom(src => src.PersonSubscriptions.GroupBy(x => x.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id).Select(x => x.First())));

        CreateMap<PersonSubscription, ReceiptRequestDto.ReceiptItem>()
            .ForMember(dest => dest.Name, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name))
            .ForMember(dest => dest.Count, cfg => cfg.MapFrom(src => 1))
            .ForMember(dest => dest.Price, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price))
            .ForMember(dest => dest.TotalPrice, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price));
    }
}
