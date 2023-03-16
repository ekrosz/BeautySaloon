using AutoMapper;
using BeautySaloon.Api.Dto.Requests.User;
using BeautySaloon.Api.Dto.Responses.User;
using WebApplication.Pages;

namespace WebApplication.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<GetUserResponseDto, EditUserComponent.UserRequest>();

        CreateMap<AddUserComponent.UserRequest, CreateUserRequestDto>();

        CreateMap<EditUserComponent.UserRequest, UpdateUserRequestDto>();
    }
}
