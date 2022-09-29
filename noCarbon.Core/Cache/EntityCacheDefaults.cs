namespace noCarbon.Core.Caching;
/// <summary>
/// Represents default values related to caching entities
/// </summary>
public static partial class EntityCacheDefaults<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Gets an entity type name used in cache keys
    /// </summary>
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    /// <summary>
    /// Gets a key for caching entity by identifier
    /// </summary>
    /// <remarks>
    /// {0} : entity id
    /// </remarks>
    public static CacheKey ByIdCacheKey => new($"SM.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

    /// <summary>
    /// Gets a key for caching entities by identifiers
    /// </summary>
    /// <remarks>
    /// {0} : entity ids
    /// </remarks>
    public static CacheKey ByIdsCacheKey => new($"SM.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

    /// <summary>
    /// Gets a key for caching all entities
    /// </summary>
    public static CacheKey AllCacheKey => new($"SM.{EntityTypeName}.all.", AllPrefix, Prefix);

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string Prefix => $"SM.{EntityTypeName}.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string ByIdPrefix => $"SM.{EntityTypeName}.byid.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string ByIdsPrefix => $"SM.{EntityTypeName}.byids.";

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    public static string AllPrefix => $"SM.{EntityTypeName}.all.";
}