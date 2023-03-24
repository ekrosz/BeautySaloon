using AutoMapper;
using BeautySaloon.Api.Dto.Requests.Subscription;
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

        CreateMap<AddOrEditSubscriptionCosmeticServiceComponent.SubscriptionCosmeticServiceRequest, EditSubscriptionComponent.SubscriptionRequest.CosmeticServiceRequest>();

        CreateMap<AddOrEditSubscriptionCosmeticServiceComponent.SubscriptionCosmeticServiceRequest, AddSubscriptionComponent.SubscriptionRequest.CosmeticServiceRequest>();

        CreateMap<EditSubscriptionComponent.SubscriptionRequest, UpdateSubscriptionRequestDto>();

        CreateMap<EditSubscriptionComponent.SubscriptionRequest.CosmeticServiceRequest, CosmeticServiceRequestDto>();

        CreateMap<EditSubscriptionComponent.SubscriptionRequest.CosmeticServiceRequest, AddOrEditSubscriptionCosmeticServiceComponent.SubscriptionCosmeticServiceRequest>();

        CreateMap<AddSubscriptionComponent.SubscriptionRequest, CreateSubscriptionRequestDto>();

        CreateMap<AddSubscriptionComponent.SubscriptionRequest.CosmeticServiceRequest, CosmeticServiceRequestDto>();

        CreateMap<AddSubscriptionComponent.SubscriptionRequest.CosmeticServiceRequest, AddOrEditSubscriptionCosmeticServiceComponent.SubscriptionCosmeticServiceRequest>();
    }
}
