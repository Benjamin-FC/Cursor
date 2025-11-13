using Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Contact entity
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
                
            entity.Property(e => e.Phone)
                .HasMaxLength(20);
                
            entity.Property(e => e.Company)
                .HasMaxLength(200);
                
            entity.Property(e => e.AddressLine1)
                .HasMaxLength(255);
                
            entity.Property(e => e.AddressLine2)
                .HasMaxLength(255);
                
            entity.Property(e => e.City)
                .HasMaxLength(100);
                
            entity.Property(e => e.State)
                .HasMaxLength(100);
                
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20);
                
            entity.Property(e => e.Country)
                .HasMaxLength(100);

            // Indexes
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => new { e.FirstName, e.LastName });
            entity.HasIndex(e => e.Company);
            entity.HasIndex(e => e.IsActive);
        });

    }
}

