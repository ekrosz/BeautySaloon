using BeautySaloon.Api.Dto.Responses.Common;
using AutoMapper;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Profiles;

public class SnapshotProfile : Profile
{
    public SnapshotProfile()
    {
        CreateMap<Subscription, SubscriptionSnapshot>();

        CreateMap<CosmeticService, CosmeticServiceSnapshot>();

        CreateMap<SubscriptionSnapshot, SubscriptionResponseDto>();

        CreateMap<CosmeticServiceSnapshot, CosmeticServiceResponseDto>();
    }
}
