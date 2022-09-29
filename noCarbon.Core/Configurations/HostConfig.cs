namespace noCarbon.Core.Configurations
{
    /// <summary>
    /// Represent hosting config
    /// </summary>
    public partial class HostConfig
    {
        /// <summary>
        /// Gets or sets enable swagger interface
        /// </summary>
        public bool EnableSwagger { get; set; }

        /// <summary>
        /// Gets or sets property name case insensitive
        /// </summary>
        public bool PropertyNameCaseInsensitive { get; set; }   
    }
}
