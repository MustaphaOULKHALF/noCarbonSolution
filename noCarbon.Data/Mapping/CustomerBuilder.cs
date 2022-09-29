using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core.Domain;
using noCarbon.Data.Mapping.Builders;

namespace noCarbon.Data.Mapping;
/// <summary>
/// Represents a Customer entity builder
/// </summary>
public partial class CustomerBuilder : EntityBuilder<Customer>
{
    #region Methods
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(nameof(Customer.Id));
        builder.Property(nameof(Customer.UserName)).HasMaxLength(256).IsRequired();
        builder.Property(nameof(Customer.Email)).HasMaxLength(256).IsRequired();    
        builder.Property(nameof(Customer.Password)).IsRequired();
        builder.Property(nameof(Customer.Avatar)).IsRequired(false);
        builder.Property(nameof(Customer.Location)).HasMaxLength(400).IsRequired(false);
        builder.Property(nameof(Customer.AllowNotification)).HasColumnType("BIT").HasDefaultValueSql("0");
        base.Configure(builder);
    }
    #endregion
}