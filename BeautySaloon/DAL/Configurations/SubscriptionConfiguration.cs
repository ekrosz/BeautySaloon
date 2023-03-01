using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class SubscriptionConfiguration : EntityConfiguration<Subscription>
{
    public override void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.LifeTimeInDays)
            .IsRequired(false);

        builder.Property(x => x.Price)
            .IsRequired();

        builder.HasMany(x => x.SubscriptionCosmeticServices)
            .WithOne(x => x.Subscription)
            .HasForeignKey("SubscriptionId")
            .IsRequired();

        builder.Navigation(x => x.SubscriptionCosmeticServices)
            .AutoInclude();

        base.Configure(builder);
    }
}
