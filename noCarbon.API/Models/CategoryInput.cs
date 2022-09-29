namespace noCarbon.API.Models;

/// <summary>
/// represent Category input
/// </summary>
public class CategoryInput
{
    /// <summary>
    /// Get or set category name 
    /// </summary>
    /// <example>Diet</example>
    public string Name { get; set; }
    /// <summary>
    /// Get or set Description
    /// </summary>
    /// <example>Diet description</example>
    public string Description { get; set; }
    /// <summary>
    /// Get or set Icon bytes
    /// </summary>
    public byte[] Icon { get; set; }
}
