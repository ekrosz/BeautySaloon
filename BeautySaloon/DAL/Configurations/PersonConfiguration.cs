using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class PersonConfiguration : EntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.OwnsOne(x => x.Name, n =>
        {
            n.WithOwner();

            n.Property(_ => _.FirstName)
            .HasMaxLength(50)
            .IsRequired();

            n.Property(_ => _.LastName)
            .HasMaxLength(50)
            .IsRequired();

            n.Property(_ => _.MiddleName)
            .HasMaxLength(50)
            .IsRequired(false);
        });

        builder.Property(x => x.BirthDate)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(12)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.Comment)
            .HasMaxLength(255)
            .IsRequired(false);

        base.Configure(builder);
    }
}
