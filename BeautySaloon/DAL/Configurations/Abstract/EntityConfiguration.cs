using BeautySaloon.DAL.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BeautySaloon.DAL.Configurations.Abstract;
public abstract class EntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class, IEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        if (typeof(TEntity).IsAssignableTo(typeof(IAuditable)))
        {
            builder.Property(nameof(IAuditable.CreatedOn))
                .IsRequired();

            builder.Property(nameof(IAuditable.UpdatedOn))
                .IsRequired();

            builder.Property(nameof(IAuditable.UserModifierId))
                .IsRequired();
        }

        if (typeof(TEntity).IsAssignableTo(typeof(ISoftDeletable)))
        {
            builder.Property(nameof(ISoftDeletable.IsDeleted))
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasQueryFilter(x => !((ISoftDeletable)x).IsDeleted);
        }
    }
}
