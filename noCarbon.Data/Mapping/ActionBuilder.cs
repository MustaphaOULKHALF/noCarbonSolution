using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core.Domain;
using noCarbon.Data.Mapping.Builders;

namespace noCarbon.Data.Mapping;
/// <summary>
/// Represents a Actions entity builder
/// </summary>
public partial class ActionsBuilder : EntityBuilder<Actions>
{
    #region Methods
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void Configure(EntityTypeBuilder<Actions> builder)
    {
        builder.HasKey(nameof(Actions.Id));
        builder.Property(nameof(Actions.CategoryId)).IsRequired();
        builder.Property(nameof(Actions.Name)).HasMaxLength(400).IsRequired();
        builder.Property(nameof(Actions.Description)).HasMaxLength(500).IsRequired(false);
        builder.Property(nameof(Actions.Points)).HasDefaultValue(0);
        builder.Property(nameof(Actions.ReducedCarb)).HasPrecision(13,2).IsRequired().HasDefaultValue(0);
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Actions)
            .HasForeignKey(nameof(Actions.CategoryId));
        base.Configure(builder);
    }
    #endregion
}