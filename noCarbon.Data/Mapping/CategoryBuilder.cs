using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using noCarbon.Core.Domain;
using noCarbon.Data.Mapping.Builders;

namespace noCarbon.Data.Mapping;
/// <summary>
/// Represents a Category entity builder
/// </summary>
public partial class CategoryBuilder : EntityBuilder<Category>
{
    #region Methods
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(nameof(Category.Id));
        builder.Property(nameof(Category.Name)).HasMaxLength(256).IsRequired();
        builder.Property(nameof(Category.Description)).HasMaxLength(500).IsRequired(false);
        builder.Property(nameof(Category.Icon)).IsRequired(false);
        base.Configure(builder);
    }
    #endregion
}