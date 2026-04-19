using Microsoft.EntityFrameworkCore;
using IcnWorksheet.Domain;

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

    // Domain entities
    public DbSet<Patient> Patients { get; set; }
    public DbSet<WardInfectionSurveillance> WardInfectionSurveillance { get; set; }
    public DbSet<Ward> Wards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Patient entity
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.HospitalNumber)
                .IsRequired()
                .HasMaxLength(50);

            // Create unique index on HospitalNumber
            entity.HasIndex(e => e.HospitalNumber)
                .IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Configure WardInfectionSurveillance entity
        modelBuilder.Entity<WardInfectionSurveillance>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.WardName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Configure foreign key to Patient
            entity.HasOne(e => e.Patient)
                .WithMany()
                .HasForeignKey(e => e.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Create index on PatientId
            entity.HasIndex(e => e.PatientId);
        });

        // Configure Ward entity
        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Group)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            // Create unique index on Name
            entity.HasIndex(e => e.Name)
                .IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}
