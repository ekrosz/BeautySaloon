using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Person;
using BeautySaloon.Api.Dto.Responses.Person;
using WebApp.Pages;

namespace WebApp.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<GetPersonResponseDto, UpdatePersonModal.Person>();

        CreateMap<UpdatePersonModal.Person, UpdatePersonRequestDto>()
            .ForMember(dest => dest.BirthDate, cfg => cfg.MapFrom(src => DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc)));

        CreateMap<CreatePersonModal.Person, CreatePersonRequestDto>()
            .ForMember(dest => dest.BirthDate, cfg => cfg.MapFrom(src => DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc)));
    }
}
