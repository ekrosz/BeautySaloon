using AutoMapper;
using BeautySaloon.Core.Dto.Responses.Auth;
using BeautySaloon.DAL.Entities;

namespace BeautySaloon.Core.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetUserResponseDto>();
    }
}
