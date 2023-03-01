using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations;

public class AppointmentConfiguration : EntityConfiguration<Appointment>
{
    public override void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.Property(x => x.AppointmentDate)
            .IsRequired();

        builder.Property(x => x.DurationInMinutes)
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
