using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core.Domain;
using noCarbon.Data.Mapping.Builders;

namespace noCarbon.Data.Mapping;

/// <summary>
/// Represents a CustomerRefreshToken entity builder
/// </summary>
public partial class CustomerRefreshTokenBuilder : EntityBuilder<CustomerRefreshToken>
{
    #region Methods
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void Configure(EntityTypeBuilder<CustomerRefreshToken> builder)
    {
        builder.HasKey(nameof(CustomerRefreshToken.Id));
        builder.Property(nameof(CustomerRefreshToken.UserName)).HasMaxLength(256).IsRequired();
        builder.Property(nameof(CustomerRefreshToken.RefreshToken)).HasMaxLength(400).IsRequired();
        builder.Property(nameof(CustomerRefreshToken.IsActive)).IsRequired();
        base.Configure(builder);
    }
    #endregion
}