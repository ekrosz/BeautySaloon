using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Person;
using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Person;
using WebApplication.Pages;

namespace WebApplication.Profiles;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<GetPersonResponseDto, EditPersonComponent.PersonRequest>();

        CreateMap<AddPersonComponent.PersonRequest, CreatePersonRequestDto>()
            .ForMember(dest => dest.BirthDate, cfg => cfg.MapFrom(src => DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc)));

        CreateMap<EditPersonComponent.PersonRequest, UpdatePersonRequestDto>()
            .ForMember(dest => dest.BirthDate, cfg => cfg.MapFrom(src => DateTime.SpecifyKind(src.BirthDate, DateTimeKind.Utc)));

        CreateMap<GetPersonResponseDto, DetailsPersonComponent.PersonRequest>();

        CreateMap<PersonSubscriptionResponseDto, DetailsPersonComponent.PersonRequest.SubscriptionRequest>();
    }
}
