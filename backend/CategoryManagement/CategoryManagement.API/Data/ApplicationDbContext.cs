using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Session> Sessions { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<SessionTag> SessionTags { get; set; }
    public DbSet<SessionSpeaker> SessionSpeakers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryCondition> CategoryConditions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure many-to-many relationships
        modelBuilder.Entity<SessionTag>()
            .HasKey(st => new { st.SessionId, st.TagId });

        modelBuilder.Entity<SessionSpeaker>()
            .HasKey(ss => new { ss.SessionId, ss.SpeakerId });

        // Index for Category conditions to improve filter performance
        modelBuilder.Entity<CategoryCondition>()
            .HasIndex(cc => new { cc.CategoryId, cc.Type });

        // Performance indices
        modelBuilder.Entity<Session>()
            .HasIndex(s => s.StartDate);

        modelBuilder.Entity<Session>()
            .HasIndex(s => s.Location);
    }
}
