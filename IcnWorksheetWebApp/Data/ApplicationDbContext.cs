using Microsoft.EntityFrameworkCore;

namespace IcnWorksheet.Data;

/// <summary>
/// Main database context for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add your DbSets here
    // public DbSet<YourEntity> YourEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure your entities here
        // modelBuilder.Entity<YourEntity>().HasKey(e => e.Id);
    }
}
