using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace noCarbon.Core.Configurations;

/// <summary>
/// Represents the app settings
/// </summary>
public partial class AppSettings
{
    #region Properties

    /// <summary>
    /// Gets or sets cache configuration parameters
    /// </summary>
    public CacheConfig CacheConfig { get; set; } = new();

    /// <summary>
    /// Gets or sets host configuration parameters
    /// </summary>
    public HostConfig HostConfig { get; set; } = new();
    
    /// <summary>
    /// Gets or sets distributed cache configuration parameters
    /// </summary>
    public DistributedCacheConfig DistributedCacheConfig { get; set; } = new();

    /// <summary>
    /// Gets or sets Security configuration parameters
    /// </summary>
    public SecurityConfig SecurityConfig { get; set; } = new();

    /// <summary>
    /// Gets or sets additional configuration parameters
    /// </summary>
    [JsonExtensionData]
    public IDictionary<string, JToken> AdditionalData { get; set; }

    #endregion
}
