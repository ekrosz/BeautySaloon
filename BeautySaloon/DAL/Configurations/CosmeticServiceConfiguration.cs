using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class CosmeticServiceConfiguration : EntityConfiguration<CosmeticService>
{
    public override void Configure(EntityTypeBuilder<CosmeticService> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.ExecuteTimeInMinutes)
            .IsRequired(false);

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasMany<SubscriptionCosmeticService>()
            .WithOne(x => x.CosmeticService)
            .HasForeignKey(x => x.CosmeticServiceId)
            .IsRequired();

        base.Configure(builder);
    }
}
