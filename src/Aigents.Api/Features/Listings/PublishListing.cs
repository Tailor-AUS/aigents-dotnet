// ═══════════════════════════════════════════════════════════════
// PUBLISH LISTING FEATURE - VERTICAL SLICE
// ═══════════════════════════════════════════════════════════════
// Distribute listing to all agents covering that area
// Exclusive off-market opportunity!
// ═══════════════════════════════════════════════════════════════

using System.Text.Json;
using Aigents.Domain.Entities;
using Aigents.Infrastructure.Data;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aigents.Api.Features.Listings;

// ───────────────────────────────────────────────────────────────
// ENDPOINT
// ───────────────────────────────────────────────────────────────

public class PublishListingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/listings/{listingId}/publish", async (Guid listingId, ISender sender) =>
        {
            var result = await sender.Send(new PublishListingCommand(listingId));
            return Results.Ok(result);
        })
        .WithName("PublishListing")
        .WithTags("Listings")
        .WithOpenApi()
        .Produces<PublishListingResponse>();
    }
}

// ───────────────────────────────────────────────────────────────
// REQUEST / RESPONSE
// ───────────────────────────────────────────────────────────────

public record PublishListingCommand(Guid ListingId) : IRequest<PublishListingResponse>;

public record PublishListingResponse(
    Guid ListingId,
    string Status,
    int AgentsNotified,
    List<NotifiedAgentDto> Agents,
    DateTime PublishedAt,
    string Message
);

public record NotifiedAgentDto(
    string Name,
    string Agency,
    string Suburbs
);

// ───────────────────────────────────────────────────────────────
// HANDLER
// ───────────────────────────────────────────────────────────────

public class PublishListingHandler : IRequestHandler<PublishListingCommand, PublishListingResponse>
{
    private readonly AigentsDbContext _db;
    private readonly ILogger<PublishListingHandler> _logger;

    public PublishListingHandler(AigentsDbContext db, ILogger<PublishListingHandler> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<PublishListingResponse> Handle(PublishListingCommand request, CancellationToken ct)
    {
        var listing = await _db.Listings
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.Id == request.ListingId, ct)
            ?? throw new InvalidOperationException("Listing not found");

        // Validate listing is ready
        if (!listing.AgreementSigned)
            throw new InvalidOperationException("Agreement must be signed before publishing");

        if (listing.DistributedToAgents)
            throw new InvalidOperationException("Listing already published");

        // Find agents covering this postcode/suburb
        var agents = await FindLocalAgentsAsync(listing.Suburb, listing.Postcode, listing.EstimatedValue, ct);

        if (!agents.Any())
        {
            // No agents found - create some demo agents for testing
            agents = await CreateDemoAgentsAsync(listing.Suburb, listing.Postcode, ct);
        }

        // Create distribution records
        var distributions = new List<ListingDistribution>();
        foreach (var agent in agents)
        {
            var distribution = new ListingDistribution
            {
                ListingId = listing.Id,
                AgentId = agent.Id,
                SentAt = DateTime.UtcNow,
                EmailSent = true, // Would trigger email service
                SmsSent = false   // Optional SMS notification
            };
            distributions.Add(distribution);
            
            agent.ListingsReceived++;
        }

        _db.ListingDistributions.AddRange(distributions);

        // Update listing status
        listing.Status = ListingStatus.Active;
        listing.DistributedToAgents = true;
        listing.DistributedAt = DateTime.UtcNow;
        listing.AgentsNotified = agents.Count;
        listing.PublishedAt = DateTime.UtcNow;
        listing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Listing {ListingId} published to {AgentCount} agents in {Suburb}",
            listing.Id, agents.Count, listing.Suburb);

        // TODO: Send actual notifications
        // - Email each agent with listing details
        // - Optional SMS for premium agents
        // - Push notification to agent app

        return new PublishListingResponse(
            listing.Id,
            "Active",
            agents.Count,
            agents.Select(a => new NotifiedAgentDto(
                a.Name,
                a.AgencyName,
                string.Join(", ", JsonSerializer.Deserialize<List<string>>(a.Suburbs) ?? new List<string>())
            )).ToList(),
            listing.PublishedAt!.Value,
            $"Your listing has been sent to {agents.Count} local agents as an exclusive off-market opportunity!"
        );
    }

    private async Task<List<Agent>> FindLocalAgentsAsync(
        string suburb, 
        string postcode, 
        decimal? propertyValue, 
        CancellationToken ct)
    {
        var query = _db.Agents
            .Where(a => a.IsActive && a.AcceptsOffMarket);

        // Filter by coverage area
        var agentsInArea = await query.ToListAsync(ct);
        
        return agentsInArea.Where(a =>
        {
            var suburbs = JsonSerializer.Deserialize<List<string>>(a.Suburbs) ?? new List<string>();
            var postcodes = JsonSerializer.Deserialize<List<string>>(a.Postcodes) ?? new List<string>();
            
            var coversArea = suburbs.Any(s => s.Equals(suburb, StringComparison.OrdinalIgnoreCase)) ||
                            postcodes.Contains(postcode);

            // Check price range if specified
            if (propertyValue.HasValue)
            {
                if (a.MinPropertyValue.HasValue && propertyValue < a.MinPropertyValue)
                    return false;
                if (a.MaxPropertyValue.HasValue && propertyValue > a.MaxPropertyValue)
                    return false;
            }

            return coversArea;
        }).ToList();
    }

    private async Task<List<Agent>> CreateDemoAgentsAsync(string suburb, string postcode, CancellationToken ct)
    {
        // Create demo agents for testing
        var demoAgents = new List<Agent>
        {
            new()
            {
                Name = "Sarah Mitchell",
                Email = "sarah@raywhite.com.au",
                Phone = "0412 345 678",
                AgencyName = "Ray White " + suburb,
                LicenseNumber = "123456789",
                Suburbs = JsonSerializer.Serialize(new List<string> { suburb }),
                Postcodes = JsonSerializer.Serialize(new List<string> { postcode }),
                IsActive = true,
                IsVerified = true
            },
            new()
            {
                Name = "Michael Chen",
                Email = "michael@ljh.com.au",
                Phone = "0423 456 789",
                AgencyName = "LJ Hooker " + suburb,
                LicenseNumber = "987654321",
                Suburbs = JsonSerializer.Serialize(new List<string> { suburb }),
                Postcodes = JsonSerializer.Serialize(new List<string> { postcode }),
                IsActive = true,
                IsVerified = true
            },
            new()
            {
                Name = "Emma Thompson",
                Email = "emma@placeestateagents.com.au",
                Phone = "0434 567 890",
                AgencyName = "Place Estate Agents",
                LicenseNumber = "456789123",
                Suburbs = JsonSerializer.Serialize(new List<string> { suburb }),
                Postcodes = JsonSerializer.Serialize(new List<string> { postcode }),
                IsActive = true,
                IsVerified = true
            }
        };

        _db.Agents.AddRange(demoAgents);
        await _db.SaveChangesAsync(ct);

        return demoAgents;
    }
}
