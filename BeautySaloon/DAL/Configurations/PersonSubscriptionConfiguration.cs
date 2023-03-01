using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class PersonSubscriptionConfiguration : EntityConfiguration<PersonSubscription>
{
    public override void Configure(EntityTypeBuilder<PersonSubscription> builder)
    {
        builder.Property(x => x.Status)
            .IsRequired();

        builder.HasOne(x => x.SubscriptionCosmeticService)
            .WithMany()
            .HasForeignKey(x => x.SubscriptionCosmeticServiceId);

        builder.Navigation(x => x.SubscriptionCosmeticService)
            .AutoInclude();

        base.Configure(builder);
    }
}
