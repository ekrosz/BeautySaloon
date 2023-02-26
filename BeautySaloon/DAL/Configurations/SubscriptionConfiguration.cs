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

        builder.Property(x => x.LifeTime)
            .IsRequired(false);

        builder.Property(x => x.Price)
            .IsRequired();

        builder.HasMany(x => x.SubscriptionCosmeticServices)
            .WithOne()
            .HasForeignKey(x => x.SubscriptionId);

        builder.Navigation(x => x.SubscriptionCosmeticServices)
            .AutoInclude();

        builder.HasMany<PersonSubscription>()
            .WithOne(x => x.Subscription)
            .HasForeignKey(x => x.SubscriptionId);

        base.Configure(builder);
    }
}
