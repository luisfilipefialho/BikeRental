using BikeRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Bike> Bikes => Set<Bike>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Rental> Rentals => Set<Rental>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bike>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => b.LicensePlate).IsUnique();
            entity.Property(b => b.Model).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => c.Cnpj).IsUnique();
            entity.HasIndex(c => c.CnhNumber).IsUnique();
            entity.Property(c => c.FullName).IsRequired();
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasOne(r => r.Bike)
                .WithMany(b => b.Rentals)
                .HasForeignKey(r => r.BikeId);

            entity.HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId);
        });
    }
}
