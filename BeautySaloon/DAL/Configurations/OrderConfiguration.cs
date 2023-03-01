using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class OrderConfiguration : EntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(x => x.Cost)
            .IsRequired();

        builder.Property(x => x.PaymentMethod)
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasMany(x => x.PersonSubscriptions)
            .WithOne()
            .HasForeignKey(x => x.OrderId);

        builder.Navigation(x => x.PersonSubscriptions)
            .AutoInclude();

        builder.Navigation(x => x.Person)
            .AutoInclude();

        base.Configure(builder);
    }
}
