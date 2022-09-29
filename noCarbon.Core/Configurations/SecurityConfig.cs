namespace noCarbon.Core.Configurations;

/// <summary>
/// Security config
/// </summary>
public class SecurityConfig
{
    /// <summary>
    /// Gets or sets an encryption key
    /// </summary>
    public string EncryptionKey { get; set; }
    /// <summary>
    /// Gets or sets a token key
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Gets or sets the issuer
    /// </summary>
    public string Issuer { get; set; }
    /// <summary>
    /// Gets or sets the audience
    /// </summary>
    public string Audience { get; set; }

}
