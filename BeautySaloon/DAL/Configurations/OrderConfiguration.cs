using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class OrderConfiguration : EntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Number)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Cost)
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .IsRequired();

        builder.Property(x => x.SpInvoiceId)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.Comment)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.HasMany(x => x.PersonSubscriptions)
            .WithOne(x => x.Order)
            .HasForeignKey("OrderId")
            .IsRequired();

        builder.Navigation(x => x.PersonSubscriptions)
            .AutoInclude();

        builder.HasOne(x => x.Modifier)
            .WithMany()
            .HasForeignKey(x => x.UserModifierId)
            .IsRequired();

        builder.Navigation(x => x.Modifier)
            .AutoInclude();

        builder.Navigation(x => x.Person)
            .AutoInclude();

        base.Configure(builder);
    }
}
