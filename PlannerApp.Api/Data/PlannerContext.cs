using Microsoft.EntityFrameworkCore;
using PlannerApp.Api.Models;


namespace PlannerApp.Api.Data
{
    public class PlannerContext : DbContext, IPlannerContext
    {
        public PlannerContext(DbContextOptions<PlannerContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlannerItem>(entity =>
            {
                entity.Property(e => e.Id).IsRequired().HasColumnName("Id");
                entity.Property(e => e.Title).IsRequired().HasColumnName("Title");
                entity.Property(e => e.Description).IsRequired().HasColumnName("Description");
                entity.Property(e => e.DateCreated).IsRequired().HasColumnName("DateCreated");
                entity.Property(e => e.DateToAction).HasColumnName("DateToAction");
                entity.Property(e => e.Completed).IsRequired().HasColumnName("Completed");
            });
            base.OnModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync()
        {
            ChangeTracker.DetectChanges();
            return await base.SaveChangesAsync();
        }

        public DbSet<PlannerItem> PlannerItems { get; set; }
    }
}
