using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.DAL.Configurations
{
    public class InvoiceConfiguration : EntityConfiguration<Invoice>
    {
        public override void Configure(EntityTypeBuilder<Invoice> builder)
        {

            builder.Property(x => x.InvoiceType)
                .IsRequired();

            builder.Property(x => x.InvoiceDate)
                .IsRequired();

            builder.Property(x => x.Comment)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .IsRequired(false);

            builder.HasMany(x => x.InvoiceMaterials)
               .WithOne(x => x.Invoice)
               .HasForeignKey("InvoiceId")
               .IsRequired();

            builder.Navigation(x => x.Employee)
                .AutoInclude();

            builder.Navigation(x => x.InvoiceMaterials)
                .AutoInclude();

            base.Configure(builder);
        }
    }
}
