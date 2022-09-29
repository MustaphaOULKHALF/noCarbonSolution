namespace noCarbon.Core;

/// <summary>
/// Reprensent the base entity
/// </summary>
public  partial class BaseEntity
{
    /// <summary>
    /// get or set identifiant 
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Get or set created date
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    /// <summary>
    /// get or set updated date
    /// </summary>
    public DateTime? UpdatedDate { get; set; }
}
