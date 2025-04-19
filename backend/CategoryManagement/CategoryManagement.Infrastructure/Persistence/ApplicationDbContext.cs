using CategoryManagement.Core.Domain;
using CategoryManagement.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Category = CategoryManagement.Core.Domain.Entities.Category;
using CategoryCondition = CategoryManagement.Core.Domain.Entities.CategoryCondition;
using Session = CategoryManagement.Core.Domain.Entities.Session;
using SessionTag = CategoryManagement.Core.Domain.Entities.SessionTag;
using Speaker = CategoryManagement.Core.Domain.Entities.Speaker;
using Tag = CategoryManagement.Core.Domain.Entities.Tag;

namespace CategoryManagement.Infrastructure.Persistence
{
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
            // Configure relationships
            modelBuilder.Entity<CategoryCondition>()
                .HasOne(cc => cc.Category)
                .WithMany(c => c.Conditions)
                .HasForeignKey(cc => cc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

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
}
