using BeautySaloon.DAL.Configurations.Abstract;
using BeautySaloon.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautySaloon.DAL.Configurations;
public class InvoiceMaterialConfiguration : EntityConfiguration<InvoiceMaterial>
{
    public override void Configure(EntityTypeBuilder<InvoiceMaterial> builder)
    {
        builder.Property(x => x.Count)
            .IsRequired();

        builder.Property(x => x.Cost)
            .IsRequired(false);

        builder.Navigation(x => x.Material)
            .AutoInclude();

        builder.Navigation(x => x.Invoice)
            .AutoInclude();

        base.Configure(builder);
    }
}

