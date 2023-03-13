using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(x => x.Login)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(x => x.Login)
            .IsUnique();

        builder.Property(x => x.Password)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.Role)
            .IsRequired();

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

        base.Configure(builder);
    }
}
