using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Appointment;
using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.Core.Profiles;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<PageResponseDto<Appointment>, PageResponseDto<GetAppointmentListItemResponseDto>>();

        CreateMap<Appointment, GetAppointmentListItemResponseDto>()
            .ForMember(dest => dest.Person, cfg => cfg.MapFrom(src => src.Person));

        CreateMap<Person, PersonResponseDto>();

        CreateMap<Appointment, GetAppointmentResponseDto>()
            .ForMember(dest => dest.Person, cfg => cfg.MapFrom(src => src.Person))
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.PersonSubscriptions));

        CreateMap<PersonSubscription, GetAppointmentResponseDto.PersonSubscriptionResponseDto>()
            .ForMember(dest => dest.Id, cfg => cfg.MapFrom(src => src.Id))
            .ForMember(dest => dest.CosmeticServiceId, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticService.CosmeticServiceId))
            .ForMember(dest => dest.CosmeticServiceName, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticService.CosmeticService.Name))
            .ForMember(dest => dest.SubscriptionId, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticService.SubscriptionId))
            .ForMember(dest => dest.SubscriptionName, cfg => cfg.MapFrom(src => src.SubscriptionCosmeticService.Subscription.Name));
    }
}
