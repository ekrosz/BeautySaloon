using AutoMapper;
using BeautySaloon.Api.Dto.Requests.User;
using BeautySaloon.Api.Dto.Responses.User;
using WebApp.Pages;

namespace WebApp.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<GetUserResponseDto, UpdateUserModal.User>();

        CreateMap<UpdateUserModal.User, UpdateUserRequestDto>();

        CreateMap<CreateUserModal.User, CreateUserRequestDto>();
    }
}
