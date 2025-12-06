using Aigents.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aigents.Infrastructure.Data;

/// <summary>
/// Entity Framework Core database context for Aigents
/// </summary>
public class AigentsDbContext : DbContext
{
    public AigentsDbContext(DbContextOptions<AigentsDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Agent> Agents => Set<Agent>();
    public DbSet<ListingInquiry> ListingInquiries => Set<ListingInquiry>();
    public DbSet<ListingDistribution> ListingDistributions => Set<ListingDistribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => new { e.Provider, e.ProviderId }).IsUnique();
            
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.InterestedSuburb).HasMaxLength(100);
            
            entity.HasMany(e => e.Conversations)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Conversation configuration
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CreatedAt);
            
            entity.Property(e => e.Summary).HasMaxLength(1000);
            
            entity.HasMany(e => e.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Message configuration
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ConversationId);
            entity.HasIndex(e => e.CreatedAt);
            
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.ModelUsed).HasMaxLength(50);
        });

        // Listing configuration
        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Suburb);
            entity.HasIndex(e => e.Postcode);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);

            entity.Property(e => e.Address).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Suburb).HasMaxLength(100).IsRequired();
            entity.Property(e => e.State).HasMaxLength(10);
            entity.Property(e => e.Postcode).HasMaxLength(10);
            entity.Property(e => e.PropertyType).HasMaxLength(50);
            entity.Property(e => e.Headline).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(5000);
            entity.Property(e => e.PriceDisplay).HasMaxLength(100);
            entity.Property(e => e.EstimatedValue).HasPrecision(18, 2);
            entity.Property(e => e.AskingPrice).HasPrecision(18, 2);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Inquiries)
                .WithOne(i => i.Listing)
                .HasForeignKey(i => i.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Agent configuration
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.IsActive);

            entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.AgencyName).HasMaxLength(255);
            entity.Property(e => e.LicenseNumber).HasMaxLength(50);
            entity.Property(e => e.MinPropertyValue).HasPrecision(18, 2);
            entity.Property(e => e.MaxPropertyValue).HasPrecision(18, 2);
            entity.Property(e => e.TotalCommissionEarned).HasPrecision(18, 2);

            entity.HasMany(e => e.Inquiries)
                .WithOne(i => i.Agent)
                .HasForeignKey(i => i.AgentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ListingInquiry configuration
        modelBuilder.Entity<ListingInquiry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ListingId);
            entity.HasIndex(e => e.AgentId);
            entity.HasIndex(e => e.CreatedAt);

            entity.Property(e => e.Message).HasMaxLength(2000);
        });

        // ListingDistribution configuration
        modelBuilder.Entity<ListingDistribution>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.ListingId, e.AgentId }).IsUnique();
            entity.HasIndex(e => e.SentAt);
        });
    }
}
