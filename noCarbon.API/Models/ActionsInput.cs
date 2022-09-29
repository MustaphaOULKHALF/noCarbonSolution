namespace noCarbon.API.Models;


/// <summary>
/// represent Action input
/// </summary>
public class ActionsInput
{
    /// <summary>
    /// Get or set the Category Id
    /// </summary>
    /// <example>1</example>
    public int CategoryId { get; set; }
    /// <summary>
    /// Get or set the name
    /// </summary>
    /// <example>Reusable bottle</example>
    public string Name { get; set; }
    /// <summary>
    /// Get or set the description
    /// </summary>
    /// <example>choosing a reusable water bottle is clearly better for the environment in countless ways</example>
    public string Description { get; set; }
    /// <summary>
    /// Get or set point
    /// </summary>
    /// <example>4</example>
    public int Points { get; set; } = 0;
    /// <summary>
    /// Get or set reduced Carb percent
    /// </summary>
    /// <example>1</example>
    public decimal ReducedCarb { get; set; } = 0;
}
