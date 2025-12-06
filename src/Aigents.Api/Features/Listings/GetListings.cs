// ═══════════════════════════════════════════════════════════════
// GET LISTINGS FEATURE - VERTICAL SLICE
// ═══════════════════════════════════════════════════════════════
// View your listings and their status
// ═══════════════════════════════════════════════════════════════

using System.Text.Json;
using Aigents.Domain.Entities;
using Aigents.Infrastructure.Data;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Aigents.Api.Features.Listings;

// ───────────────────────────────────────────────────────────────
// ENDPOINTS
// ───────────────────────────────────────────────────────────────

public class GetListingsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Get user's listings
        app.MapGet("/api/listings/my/{userId}", async (Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetMyListingsQuery(userId));
            return Results.Ok(result);
        })
        .WithName("GetMyListings")
        .WithTags("Listings")
        .WithOpenApi()
        .Produces<List<ListingDto>>();

        // Get single listing
        app.MapGet("/api/listings/{listingId}", async (Guid listingId, ISender sender) =>
        {
            var result = await sender.Send(new GetListingQuery(listingId));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetListing")
        .WithTags("Listings")
        .WithOpenApi()
        .Produces<ListingDetailDto>();

        // Update listing
        app.MapPut("/api/listings/{listingId}", async (Guid listingId, UpdateListingRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateListingCommand(
                listingId,
                request.Headline,
                request.Description,
                request.Features,
                request.AskingPrice,
                request.PriceDisplay
            ));
            return Results.Ok(result);
        })
        .WithName("UpdateListing")
        .WithTags("Listings")
        .WithOpenApi()
        .Produces<ListingDto>();
    }
}

// ───────────────────────────────────────────────────────────────
// DTOs
// ───────────────────────────────────────────────────────────────

public record ListingDto(
    Guid Id,
    string Address,
    string Suburb,
    string PropertyType,
    string Headline,
    string PriceDisplay,
    string Status,
    int AgentsNotified,
    int InquiriesCount,
    DateTime CreatedAt
);

public record ListingDetailDto(
    Guid Id,
    string Address,
    string Suburb,
    string Postcode,
    string PropertyType,
    int? Bedrooms,
    int? Bathrooms,
    int? CarSpaces,
    int? LandSize,
    string Headline,
    string Description,
    List<string> Features,
    string PriceDisplay,
    decimal? EstimatedValue,
    decimal? AskingPrice,
    string TargetBuyers,
    string Status,
    bool AgreementSigned,
    DateTime? AgreementSignedAt,
    bool DistributedToAgents,
    int AgentsNotified,
    List<InquiryDto> Inquiries,
    DateTime CreatedAt,
    DateTime? PublishedAt
);

public record InquiryDto(
    Guid Id,
    string AgentName,
    string AgencyName,
    string Message,
    string Status,
    DateTime CreatedAt
);

// ───────────────────────────────────────────────────────────────
// GET MY LISTINGS
// ───────────────────────────────────────────────────────────────

public record GetMyListingsQuery(Guid UserId) : IRequest<List<ListingDto>>;

public class GetMyListingsHandler : IRequestHandler<GetMyListingsQuery, List<ListingDto>>
{
    private readonly AigentsDbContext _db;

    public GetMyListingsHandler(AigentsDbContext db) => _db = db;

    public async Task<List<ListingDto>> Handle(GetMyListingsQuery request, CancellationToken ct)
    {
        return await _db.Listings
            .Where(l => l.UserId == request.UserId)
            .OrderByDescending(l => l.CreatedAt)
            .Select(l => new ListingDto(
                l.Id,
                l.Address,
                l.Suburb,
                l.PropertyType,
                l.Headline,
                l.PriceDisplay,
                l.Status.ToString(),
                l.AgentsNotified,
                l.Inquiries.Count,
                l.CreatedAt
            ))
            .ToListAsync(ct);
    }
}

// ───────────────────────────────────────────────────────────────
// GET SINGLE LISTING
// ───────────────────────────────────────────────────────────────

public record GetListingQuery(Guid ListingId) : IRequest<ListingDetailDto?>;

public class GetListingHandler : IRequestHandler<GetListingQuery, ListingDetailDto?>
{
    private readonly AigentsDbContext _db;

    public GetListingHandler(AigentsDbContext db) => _db = db;

    public async Task<ListingDetailDto?> Handle(GetListingQuery request, CancellationToken ct)
    {
        var listing = await _db.Listings
            .Include(l => l.Inquiries)
                .ThenInclude(i => i.Agent)
            .FirstOrDefaultAsync(l => l.Id == request.ListingId, ct);

        if (listing is null) return null;

        var features = JsonSerializer.Deserialize<List<string>>(listing.Features) ?? new List<string>();

        return new ListingDetailDto(
            listing.Id,
            listing.Address,
            listing.Suburb,
            listing.Postcode,
            listing.PropertyType,
            listing.Bedrooms,
            listing.Bathrooms,
            listing.CarSpaces,
            listing.LandSize,
            listing.Headline,
            listing.Description,
            features,
            listing.PriceDisplay,
            listing.EstimatedValue,
            listing.AskingPrice,
            listing.TargetBuyers,
            listing.Status.ToString(),
            listing.AgreementSigned,
            listing.AgreementSignedAt,
            listing.DistributedToAgents,
            listing.AgentsNotified,
            listing.Inquiries.Select(i => new InquiryDto(
                i.Id,
                i.Agent.Name,
                i.Agent.AgencyName,
                i.Message,
                i.Status.ToString(),
                i.CreatedAt
            )).ToList(),
            listing.CreatedAt,
            listing.PublishedAt
        );
    }
}

// ───────────────────────────────────────────────────────────────
// UPDATE LISTING
// ───────────────────────────────────────────────────────────────

public record UpdateListingRequest(
    string? Headline,
    string? Description,
    List<string>? Features,
    decimal? AskingPrice,
    string? PriceDisplay
);

public record UpdateListingCommand(
    Guid ListingId,
    string? Headline,
    string? Description,
    List<string>? Features,
    decimal? AskingPrice,
    string? PriceDisplay
) : IRequest<ListingDto>;

public class UpdateListingHandler : IRequestHandler<UpdateListingCommand, ListingDto>
{
    private readonly AigentsDbContext _db;

    public UpdateListingHandler(AigentsDbContext db) => _db = db;

    public async Task<ListingDto> Handle(UpdateListingCommand request, CancellationToken ct)
    {
        var listing = await _db.Listings.FindAsync([request.ListingId], ct)
            ?? throw new InvalidOperationException("Listing not found");

        if (listing.Status != ListingStatus.Draft && listing.Status != ListingStatus.PendingSignature)
            throw new InvalidOperationException("Can only edit draft listings");

        // Update fields if provided
        if (request.Headline is not null) listing.Headline = request.Headline;
        if (request.Description is not null) listing.Description = request.Description;
        if (request.Features is not null) listing.Features = JsonSerializer.Serialize(request.Features);
        if (request.AskingPrice.HasValue) listing.AskingPrice = request.AskingPrice;
        if (request.PriceDisplay is not null) listing.PriceDisplay = request.PriceDisplay;

        listing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return new ListingDto(
            listing.Id,
            listing.Address,
            listing.Suburb,
            listing.PropertyType,
            listing.Headline,
            listing.PriceDisplay,
            listing.Status.ToString(),
            listing.AgentsNotified,
            0, // Inquiries count - would need to query
            listing.CreatedAt
        );
    }
}
