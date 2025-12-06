namespace Aigents.Domain.Entities;

/// <summary>
/// Represents a user/lead in the system
/// </summary>
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? AvatarUrl { get; set; }
    public string Provider { get; set; } = "google"; // google, apple, email
    public string? ProviderId { get; set; }
    
    // Lead info
    public string? InterestedSuburb { get; set; }
    public AgentMode PreferredMode { get; set; } = AgentMode.Buy;
    public LeadStatus Status { get; set; } = LeadStatus.New;
    public DateTime? HandedOffAt { get; set; }
    public string? AssignedAgentId { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastActiveAt { get; set; }
    
    // Navigation
    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
}

public enum AgentMode
{
    Buy,
    Sell
}

public enum LeadStatus
{
    New,
    Engaged,
    Qualified,
    HandedOff,
    Converted,
    Lost
}
