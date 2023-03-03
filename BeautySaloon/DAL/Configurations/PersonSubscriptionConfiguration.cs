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

        builder.HasOne<SubscriptionCosmeticService>()
            .WithMany()
            .HasForeignKey(x => x.SubscriptionCosmeticServiceId);

        builder.OwnsOne(x => x.SubscriptionCosmeticServiceSnapshot, s =>
        {
            s.WithOwner();

            s.Property(x => x.Count)
                .IsRequired();

            s.OwnsOne(x => x.SubscriptionSnapshot, subscription =>
            {
                subscription.WithOwner();

                subscription.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                subscription.Property(x => x.LifeTimeInDays)
                    .IsRequired(false);

                subscription.Property(x => x.Price)
                    .IsRequired();
            });

            s.OwnsOne(x => x.CosmeticServiceSnapshot, service =>
            {
                service.WithOwner();

                service.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                service.Property(x => x.ExecuteTimeInMinutes)
                    .IsRequired();

                service.Property(x => x.Description)
                    .HasMaxLength(500)
                    .IsRequired();
            });
        });

        base.Configure(builder);
    }
}
