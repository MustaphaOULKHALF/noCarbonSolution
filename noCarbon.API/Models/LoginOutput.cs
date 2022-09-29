namespace noCarbon.API.Models;

/// <summary>
/// Get or set login result
/// </summary>
public class LoginOutput
{
    /// <summary>
    /// Get or set the token value
    /// </summary>
    public string Token { get; set; }
    /// <summary>
    /// Get or set refresh token
    /// </summary>
    public string RefreshToken { get; set; }
}
