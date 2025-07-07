using Microsoft.EntityFrameworkCore;
using WebApplication10.models;

namespace WebApplication10.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plan>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Plans)
                .UsingEntity(j => j.ToTable("PlanTags"));
        }
    }
}
