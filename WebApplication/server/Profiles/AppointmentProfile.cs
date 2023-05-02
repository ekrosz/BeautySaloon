using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Dto.Responses.Appointment;
using WebApplication.Pages;

namespace WebApplication.Profiles;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<AddAppointmentComponent.AppointmentRequest, CreateAppointmentRequestDto>()
            .ForMember(dest => dest.AppointmentDate, cfg => cfg.MapFrom(src => DateTime.SpecifyKind(src.AppointmentDate, DateTimeKind.Local)));

        CreateMap<EditAppointmentComponent.AppointmentRequest, UpdateAppointmentRequestDto>()
            .ForMember(dest => dest.AppointmentDate, cfg => cfg.MapFrom(src => DateTime.SpecifyKind(src.AppointmentDate, DateTimeKind.Local)));

        CreateMap<GetAppointmentResponseDto, EditAppointmentComponent.AppointmentRequest>()
            .ForMember(dest => dest.AppointmentDate, cfg => cfg.MapFrom(src => src.AppointmentDate))
            .ForMember(dest => dest.PersonSubscriptionIds, cfg => cfg.MapFrom(src => src.Subscriptions.Select(x => x.Id)));
    }
}
