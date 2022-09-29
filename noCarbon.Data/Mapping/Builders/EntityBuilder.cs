using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core;

namespace noCarbon.Data.Mapping.Builders;
/// <summary>
/// Represents base entity builder
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
/// <remarks>
/// "Entity type <see cref="TEntity"/>" is needed to determine the right entity builder for a specific entity type
/// </remarks>
public abstract class EntityBuilder<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.ToTable(typeof(TEntity).Name);
        if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) != null)
        {
            builder.Property(nameof(ISoftDeletedEntity.Deleted)).HasColumnType("BIT").HasDefaultValueSql("0");
            builder.Property(nameof(ISoftDeletedEntity.DeletedDate)).HasColumnType("DATETIME").IsRequired(false);
        }
        builder.Property(nameof(BaseEntity.CreatedDate)).HasColumnType("DATETIME").IsRequired();
        builder.Property(nameof(BaseEntity.UpdatedDate)).HasColumnType("DATETIME").IsRequired(false);
        builder.Property(nameof(BaseEntity.CreatedDate)).HasColumnType("DATETIME").IsRequired();
        
    }
}
