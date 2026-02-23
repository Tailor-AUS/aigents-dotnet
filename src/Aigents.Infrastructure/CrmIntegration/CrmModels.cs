namespace Aigents.Infrastructure.CrmIntegration;

/// <summary>
/// Represents a contact normalized from any CRM system.
/// </summary>
public record CrmContact
{
    public string ExternalId { get; init; } = string.Empty;
    public string CrmSource { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Mobile { get; init; }
    public ContactClassification Classification { get; init; } = ContactClassification.Unknown;
    public string? LeadSource { get; init; }
    public DateTimeOffset? LastContactDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; init; }
    public Dictionary<string, string> CustomFields { get; init; } = new();
}

public enum ContactClassification
{
    Unknown,
    Buyer,
    Seller,
    Investor,
    Tenant,
    Landlord,
    Vendor,
    OtherAgent
}

/// <summary>
/// Represents a property listing normalized from any CRM system.
/// </summary>
public record CrmProperty
{
    public string ExternalId { get; init; } = string.Empty;
    public string CrmSource { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string? Suburb { get; init; }
    public string? State { get; init; }
    public string? Postcode { get; init; }
    public PropertyType Type { get; init; } = PropertyType.House;
    public ListingStatus Status { get; init; } = ListingStatus.Active;
    public decimal? PriceFrom { get; init; }
    public decimal? PriceTo { get; init; }
    public string? PriceDisplay { get; init; }
    public int? Bedrooms { get; init; }
    public int? Bathrooms { get; init; }
    public int? CarSpaces { get; init; }
    public string? AgentId { get; init; }
    public DateTimeOffset? ListedDate { get; init; }
}

public enum PropertyType
{
    House,
    Unit,
    Apartment,
    Townhouse,
    Land,
    Rural,
    Commercial
}

public enum ListingStatus
{
    Active,
    UnderContract,
    Sold,
    Withdrawn,
    OffMarket
}

/// <summary>
/// Represents an interaction/activity to be logged to a CRM.
/// </summary>
public record CrmActivity
{
    public string? ContactId { get; init; }
    public string? PropertyId { get; init; }
    public ActivityType Type { get; init; } = ActivityType.Note;
    public string Subject { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public int? DurationSeconds { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = new();
}

public enum ActivityType
{
    Note,
    Call,
    Email,
    SMS,
    Inspection,
    Meeting,
    Task
}

/// <summary>
/// Represents a task to be created in a CRM.
/// </summary>
public record CrmTask
{
    public string? ContactId { get; init; }
    public string? PropertyId { get; init; }
    public string Subject { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTimeOffset? DueDate { get; init; }
    public TaskPriority Priority { get; init; } = TaskPriority.Normal;
    public string? AssignedToAgentId { get; init; }
}

public enum TaskPriority
{
    Low,
    Normal,
    High,
    Urgent
}

/// <summary>
/// Represents an open home inspection event.
/// </summary>
public record CrmInspection
{
    public string ExternalId { get; init; } = string.Empty;
    public string PropertyId { get; init; } = string.Empty;
    public string? PropertyAddress { get; init; }
    public DateTimeOffset StartTime { get; init; }
    public DateTimeOffset EndTime { get; init; }
    public string? AgentId { get; init; }
    public int? RsvpCount { get; init; }
}
