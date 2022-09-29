namespace noCarbon.API.Models;

/// <summary>
/// Represent refresh token request
/// </summary>
public class RefreshTokenInput
{
    /// <summary>
    /// Get or set current token 
    /// </summary>
    public string AccessToken { get; set; }
    /// <summary>
    /// Get or set private refresh token
    /// </summary>
    public string RefreshToken { get; set; }
}
