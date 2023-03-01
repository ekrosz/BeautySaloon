using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class SubscriptionCosmeticServiceConfiguration : EntityConfiguration<SubscriptionCosmeticService>
{
    public override void Configure(EntityTypeBuilder<SubscriptionCosmeticService> builder)
    {
        builder.Navigation(x => x.CosmeticService)
            .AutoInclude();

        builder.Navigation(x => x.Subscription)
            .AutoInclude();

        base.Configure(builder);
    }
}
