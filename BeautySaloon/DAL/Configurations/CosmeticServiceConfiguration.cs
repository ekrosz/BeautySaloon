using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasMany<SubscriptionCosmeticService>()
            .WithOne(x => x.CosmeticService)
            .HasForeignKey(x => x.CosmeticServiceId);

        base.Configure(builder);
    }
}
