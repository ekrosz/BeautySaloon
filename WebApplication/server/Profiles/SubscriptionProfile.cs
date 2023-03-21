using AutoMapper;
using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.Api.Dto.Responses.Subscription;
using WebApplication.Pages;

namespace WebApplication.Profiles;

public class SubscriptionProfile : Profile
{
    public SubscriptionProfile()
    {
        CreateMap<GetSubscriptionResponseDto, EditSubscriptionComponent.SubscriptionRequest>();

        CreateMap<CosmeticServiceResponseDto, EditSubscriptionComponent.SubscriptionRequest.CosmeticServiceRequest>();
    }
}
