# Buyer Data Integration Research

## Overview
To empower the "Buyer" persona, Aigents needs robust property data coverage including:
1.  **On-Market Listings**: Active listings for sale/rent (Domain, REA).
2.  **Property Intelligence**: Ownership, sales history, AVMs (RP Data/CoreLogic, Pricefinder).
3.  **Market Data**: Suburb trends, clearance rates (PropTrack, SQM).

## 1. Domain API (Listings & Content)
**Status**: Public API Available
**Docs**: https://developer.domain.com.au/docs/latest/
**Auth**: OAuth2 (Client Credentials for agents, PKCE for users)

### Key Endpoints
-   `GET /v1/listings/residential/_search`: Search active listings.
-   `GET /v1/listings/{id}`: Detailed listing data (photos, floorplans).
-   `GET /v1/salesResults/{city}`: Weekend auction results.
-   `GET /v1/suburbPerformanceStatistics`: Median price, growth data.
-   `GET /v1/demographics`: Suburb profiles.

### Integration Strategy
-   **Adapter**: `DomainPropertyAdapter`
-   **Capabilities**: Live search, specific property lookup, suburb stats.
-   **Cost**: Free tier available, tiered pricing for high volume.

## 2. CoreLogic / RP Data (Property Intelligence)
**Status**: Enterprise Only (BSG/RP Professional)
**Docs**: https://miktsh.atlassian.net/wiki/spaces/RPDATA/overview (Unofficial/Partner access usually required)
**Access**: Requires specific commercial agreement (Property Innovation API).

### Key Features
-   **AVM (Automated Valuation Model)**: High-accuracy property value estimates.
-   **Sales History**: Past transfer data (essential for "what's it worth").
-   **On-the-market status**: Aggregated from multiple sources.
-   **Ownership**: Owner names and contact details (privacy restricted).

### Integration Strategy
-   **Adapter**: `CoreLogicAdapter`
-   **Mocking**: Since this requires expensive enterprise keys, we will build a full interface but mock the implementation for development using realistic data structures.

## 3. realestate.com.au (REA Group)
**Status**: Restrictive Public API
**Docs**: https://api.realestate.com.au/ (Requires partner access)
-   REA is notoriously protective of data. Integration usually happens via specific partner programs or strictly for agency use only (Feed APIs).
-   **Alternative**: Web scraping (Not recommended - legal gray area, prone to blocks).
-   **Strategy**: Focus on Domain for "Search" as their API is more developer-friendly. Use REA XML feed parsing if the user (Agent) provides their own data feed.

## 4. Open Data (Mapping & Amenities)
-   **Mapbox / Azure Maps**: For visualization.
-   **Google Places**: For "Nearby Analytics" (Schools, Shops).
-   **ABS (Australian Bureau of Statistics)**: Census data for suburb profiles (Free).

---

## Technical Architecture

### Unified Property Model (`BuyerProperty`)
We need a normalized model that blends listing data with historical data.

```csharp
public class BuyerProperty
{
    // Identifiers
    public string Id { get; set; }
    public string Address { get; set; }
    public GeoLocation Location { get; set; }
    
    // Listing Details (Dynamic)
    public bool IsOnMarket { get; set; }
    public decimal? ListingPrice { get; set; }
    public string Agency { get; set; }
    public string ListingUrl { get; set; }
    
    // Core Data (Static/Historical)
    public PropertyType Type { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int CarSpaces { get; set; }
    public double LandSizeSqm { get; set; }
    
    // Intelligence
    public decimal? EstimatedValue { get; set; } // AVM
    public decimal? LastSalePrice { get; set; }
    public DateTime? LastSaleDate { get; set; }
    public decimal? RentalEstimate { get; set; }
}
```

### Interface: `IPropertyDataProvider`
```csharp
public interface IPropertyDataProvider
{
    Task<SearchResults> SearchAsync(PropertySearchFilter filter);
    Task<BuyerProperty> GetDetailsAsync(string propertyId);
    Task<PropertyValuation> GetValuationAsync(string address);
    Task<SuburbProfile> GetSuburbProfileAsync(string suburb, string state);
}
```

## Implementation Plan
1.  **Domain Adapter**: Implement real connection for listings.
2.  **CoreLogic Mock**: Implement high-fidelity mock for specific test addresses.
3.  **Aggregation Service**: `PropertyIntelligenceService` that merges data from both.
```
