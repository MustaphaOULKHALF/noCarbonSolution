using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core.Domain;
using noCarbon.Data.Mapping.Builders;

namespace noCarbon.Data.Mapping;
/// <summary>
/// Represents a Historic entity builder
/// </summary>
public partial class HistoricBuilder : EntityBuilder<Historic>
{
    #region Methods
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void Configure(EntityTypeBuilder<Historic> builder)
    {
        builder.HasKey(nameof(Historic.Id));
        builder.Property(nameof(Historic.CustomerId)).IsRequired();
        builder.Property(nameof(Historic.CategoryId)).IsRequired();
        builder.Property(nameof(Historic.ActionId)).IsRequired();
        builder.Property(nameof(Historic.Points)).HasDefaultValue(0);
        builder.Property(nameof(Historic.ReducedCarb)).HasPrecision(13, 2).IsRequired().HasDefaultValue(0);
        builder.Property(nameof(Historic.OperationDate)).HasColumnType("DATE").IsRequired();
        builder.Property(nameof(Historic.OperationTime)).HasColumnType("TIME").IsRequired();
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Historics)
            .HasForeignKey(nameof(Historic.CategoryId))
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Historics)
            .HasForeignKey(nameof(Historic.CustomerId))
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Action)
            .WithMany(x => x.Historics)
            .HasForeignKey(nameof(Historic.ActionId))
            .OnDelete(DeleteBehavior.NoAction);
        base.Configure(builder);
    }
    #endregion
}