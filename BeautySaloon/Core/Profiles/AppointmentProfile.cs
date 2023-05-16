using BeautySaloon.Api.Dto.Responses.Appointment;
using BeautySaloon.Api.Dto.Responses.Common;
using AutoMapper;
using BeautySaloon.DAL.Entities;

namespace BeautySaloon.Core.Profiles;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, GetAppointmentListItemResponseDto>()
            .ForMember(dest => dest.Person, cfg => cfg.MapFrom(src => src.Person))
            .ForMember(dest => dest.Modifier, cfg => cfg.MapFrom(src => src.Modifier));

        CreateMap<Person, PersonResponseDto>();

        CreateMap<Appointment, GetAppointmentResponseDto>()
            .ForMember(dest => dest.Person, cfg => cfg.MapFrom(src => src.Person))
            .ForMember(dest => dest.Modifier, cfg => cfg.MapFrom(src => src.Modifier))
            .ForMember(dest => dest.Subscriptions, cfg => cfg.MapFrom(src => src.PersonSubscriptions));
    }
}
