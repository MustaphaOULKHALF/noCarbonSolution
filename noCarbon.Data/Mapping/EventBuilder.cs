using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core.Domain;
using noCarbon.Data.Mapping.Builders;

namespace noCarbon.Data.Mapping;
/// <summary>
/// Represents a Events entity builder
/// </summary>
public partial class EventBuilder : EntityBuilder<Events>
{
    #region Methods
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void Configure(EntityTypeBuilder<Events> builder)
    {
        builder.HasKey(nameof(Events.Id));
        builder.Property(nameof(Events.Title)).HasMaxLength(400).IsRequired();
        builder.Property(nameof(Events.Body)).HasMaxLength(500).IsRequired(false);
        builder.Property(nameof(Events.Image)).IsRequired(false);
        builder.Property(nameof(Events.StartDate)).IsRequired(false);
        builder.Property(nameof(Events.EndDate)).IsRequired(false);
        base.Configure(builder);
    }
    #endregion
}