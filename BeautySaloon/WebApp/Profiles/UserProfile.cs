using AutoMapper;
using BeautySaloon.Api.Dto.Requests.User;
using BeautySaloon.Api.Dto.Responses.User;

namespace WebApp.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<GetUserResponseDto, UpdateUserRequestDto>();
    }
}
