using BeautySaloon.DAL.Configurations.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeautySaloon.DAL.Entities;

namespace BeautySaloon.DAL.Configurations
{
    public class MaterialConfiguration : EntityConfiguration<Material>
    {
        public override void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasMany<InvoiceMaterial>()
                .WithOne(x => x.Material)
                .HasForeignKey(x => x.MaterialId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
