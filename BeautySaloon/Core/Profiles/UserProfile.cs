using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.User;
using AutoMapper;
using BeautySaloon.DAL.Entities;

namespace BeautySaloon.Core.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, GetUserResponseDto>();

        CreateMap<User, ModifierResponseDto>();
    }
}
