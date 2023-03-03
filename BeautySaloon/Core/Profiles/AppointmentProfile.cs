using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Appointment;
using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Profiles;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<PageResponseDto<Appointment>, PageResponseDto<GetAppointmentListItemResponseDto>>();

        CreateMap<Appointment, GetAppointmentListItemResponseDto>()
            .ForMember(dest => dest.Person, cfg => cfg.MapFrom(src => src.Person))
            .ForMember(dest => dest.Modifier, cfg => cfg.MapFrom(src => src.Modifier));

        CreateMap<Person, PersonResponseDto>();

        CreateMap<Appointment, GetAppointmentResponseDto>()
            .ForMember(dest => dest.Person, cfg => cfg.MapFrom(src => src.Person))
            .ForMember(dest => dest.Modifier, cfg => cfg.MapFrom(src => src.Modifier))
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.PersonSubscriptions));

        CreateMap<PersonSubscription, PersonSubscriptionResponseDto>()
            .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.Id))
            .ForMember(dest => dest.Subscription, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot))
            .ForMember(dest => dest.CosmeticService, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticServiceSnapshot.CosmeticServiceSnapshot));
    }
}
