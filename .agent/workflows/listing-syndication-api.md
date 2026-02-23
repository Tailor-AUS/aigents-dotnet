---
description: Implementation plan for multi-platform listing syndication APIs
---

# Listing Syndication API Implementation Plan

## Business Model
- **Agents Portal**: Real estate agents from any agency list properties, earn commission per listing
- **Sell Portal**: Private owners list their properties, pay listing fee
- Both get their listing syndicated to multiple platforms from a single submission

## Platform Integration Status

### Tier 1: Official APIs (Implement First)
| Platform | API Type | Format | Auth | Documentation |
|----------|----------|--------|------|---------------|
| Domain.com.au | Listing Management API | JSON | OAuth2 | developer.domain.com.au |
| realestate.com.au | Listing Upload API | REAXML | Bearer Token | partner.realestate.com.au |

### Tier 2: Limited/Manual (Future Phase)
| Platform | Status | Approach |
|----------|--------|----------|
| Facebook Marketplace | Marketing API only | Home Listing Catalog for ads (requires ad spend) |
| Gumtree | No public API | Manual workflow or browser automation |
| Flatmates.com.au | Research needed | Check for API availability |

## Implementation Steps

### Phase 1: Core Listing Service (Week 1-2)
1. Create unified `Listing` entity model
2. Build `IListingService` interface
3. Implement listing CRUD operations
4. Create listing form UI for Agents/Sellers

### Phase 2: Domain API Integration (Week 2-3)
1. Register at developer.domain.com.au
2. Create sandbox project
3. Implement Domain API client
4. Implement listing upload/sync
5. Test with sandbox agency

### Phase 3: REA API Integration (Week 3-4)
1. Apply for partner access at partner.realestate.com.au
2. Implement REAXML format transformer
3. Build REA API client
4. Testing in sandbox environment

### Phase 4: AI Enhancement (Week 4-5)
1. AI-generated listing descriptions
2. Image optimization/enhancement
3. Pricing recommendations based on market data
4. Auto-scheduling for optimal listing times

## Technical Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     AIGENTS PLATFORM                        │
├─────────────────────────────────────────────────────────────┤
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐     │
│  │   Agents    │    │   Sellers   │    │    Admin    │     │
│  │   Portal    │    │   Portal    │    │   Portal    │     │
│  └──────┬──────┘    └──────┬──────┘    └──────┬──────┘     │
│         │                  │                  │             │
│  ┌──────┴──────────────────┴──────────────────┴──────┐     │
│  │              LISTING SERVICE                       │     │
│  │  • Create/Edit/Delete Listings                     │     │
│  │  • AI Description Generator                        │     │
│  │  • Image Processing                                │     │
│  │  • Commission Calculator                           │     │
│  └──────────────────────▲────────────────────────────┘     │
│                         │                                   │
│  ┌──────────────────────┴────────────────────────────┐     │
│  │           SYNDICATION ENGINE                       │     │
│  │  • Platform Adapters                               │     │
│  │  • Status Tracking                                 │     │
│  │  • Error Handling & Retry                          │     │
│  └────┬─────────┬─────────┬─────────┬───────────────┘     │
│       │         │         │         │                      │
└───────┼─────────┼─────────┼─────────┼──────────────────────┘
        │         │         │         │
   ┌────▼────┐ ┌──▼──┐ ┌────▼────┐ ┌──▼──┐
   │ Domain  │ │ REA │ │Facebook │ │ ... │
   │  API    │ │ API │ │  Ads    │ │     │
   └─────────┘ └─────┘ └─────────┘ └─────┘
```

## Data Models

### Listing Entity
```csharp
public class Listing
{
    public Guid Id { get; set; }
    public Guid AgentId { get; set; } // Or SellerId for private
    public ListingType Type { get; set; } // ForSale, ForRent
    public PropertyType PropertyType { get; set; } // House, Unit, Land
    
    // Address
    public string StreetAddress { get; set; }
    public string Suburb { get; set; }
    public string State { get; set; }
    public string Postcode { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    // Details
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int CarSpaces { get; set; }
    public double? LandAreaSqm { get; set; }
    public double? BuildingAreaSqm { get; set; }
    
    // Pricing
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public string PriceDisplay { get; set; } // "Offers Over $850k"
    
    // Content
    public string Headline { get; set; }
    public string Description { get; set; }
    public List<string> Features { get; set; }
    public List<ListingImage> Images { get; set; }
    
    // Syndication Status
    public List<SyndicationRecord> Syndications { get; set; }
    
    // Financials
    public decimal ListingFee { get; set; }
    public decimal AgentCommission { get; set; }
}

public class SyndicationRecord
{
    public string Platform { get; set; } // "domain", "rea", "facebook"
    public SyndicationStatus Status { get; set; }
    public string ExternalListingId { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string ErrorMessage { get; set; }
}
```

## API Credentials Required

### Domain.com.au
- Client ID (from developer portal)
- Client Secret
- Sandbox Agency ID (for testing)
- Live Agency ID (for production)

### realestate.com.au
- Partner API Key
- Agency ID
- REAXML credentials

## Revenue Model Implementation

### For Agents
- Customer pays listing fee: $XXX
- Agent receives commission: 12% of listing fee ($XXX × 0.12)
- Platform revenue: Remaining 88%

### For Private Sellers
- Base listing fee: $XXX
- Platform keeps 100%
- Optional add-ons (premium placement, more photos, etc.)

## Next Steps
1. ✅ Update UI to focus on Agents + Sell portals
2. Create `Listing` entity and database migrations
3. Build listing creation form
4. Register for Domain API sandbox access
5. Implement Domain API client
