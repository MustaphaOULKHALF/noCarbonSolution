using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core.Domain;
using noCarbon.Data.Mapping.Builders;

namespace noCarbon.Data.Mapping;
/// <summary>
/// Represents a Sponsorship entity builder
/// </summary>
public partial class SponsorshipBuilder : EntityBuilder<Sponsorship>
{
    #region Methods
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void Configure(EntityTypeBuilder<Sponsorship> builder)
    {
        builder.HasKey(nameof(Sponsorship.Id));
        builder.Property(nameof(Sponsorship.CustomerId)).IsRequired();
        builder.Property(nameof(Sponsorship.Token)).HasMaxLength(500).IsRequired();
        builder.Property(nameof(Sponsorship.ExpiryDate)).IsRequired(false);
        builder.Property(nameof(Sponsorship.Message)).IsRequired(false);
        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Sponsorships)
            .HasForeignKey(nameof(Sponsorship.CustomerId));
        base.Configure(builder);
    }
    #endregion
}