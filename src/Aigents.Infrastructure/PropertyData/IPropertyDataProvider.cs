namespace Aigents.Infrastructure.PropertyData;

/// <summary>
/// Provider for property data, abstracts multiple sources (Domain, CoreLogic, etc.)
/// </summary>
public interface IPropertyDataProvider
{
    /// <summary>
    /// Identifier for this provider (e.g., "domain", "corelogic")
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Search for properties based on criteria
    /// </summary>
    Task<SearchResults> SearchAsync(PropertySearchFilter filter, CancellationToken ct = default);

    /// <summary>
    /// Get details for a specific property by ID
    /// </summary>
    Task<BuyerProperty?> GetByIdAsync(string id, CancellationToken ct = default);

    /// <summary>
    /// Get details by address (for off-market lookups)
    /// </summary>
    Task<BuyerProperty?> GetByAddressAsync(string address, CancellationToken ct = default);

    /// <summary>
    /// Get market insights for a suburb
    /// </summary>
    Task<SuburbProfile?> GetSuburbProfileAsync(string suburb, string state, CancellationToken ct = default);
}
