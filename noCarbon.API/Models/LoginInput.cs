namespace noCarbon.API.Models;
/// <summary>
/// represent Login input
/// </summary>
public class LoginInput
{
    /// <summary>
    /// Get or set User name
    /// </summary>
    /// <example>moulkhalf</example>
    public string Username { get; set; }
    /// <summary>
    /// Get or set password
    /// </summary>
    /// <example>UkZQdkxhu9o8cGsqQDijKBAkGNPSS5FI</example>
    public string Password { get; set; }
}