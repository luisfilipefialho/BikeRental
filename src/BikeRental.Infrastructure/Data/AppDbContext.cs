using BikeRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Bike> Bikes => Set<Bike>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<Notify> Notifications { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bike>(entity =>
        {
            entity.HasKey(b => b.Identifier);
            entity.HasIndex(b => b.LicensePlate).IsUnique();
            entity.Property(b => b.Model).IsRequired();
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(c => c.Identifier);
            entity.HasIndex(c => c.Cnpj).IsUnique();
            entity.HasIndex(c => c.CnhNumber).IsUnique();
            entity.Property(c => c.FullName).IsRequired();
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(r => r.Identifier);

            entity.HasOne(r => r.Bike)
                .WithMany(b => b.Rentals)
                .HasForeignKey(r => r.BikeId);

            entity.HasOne(r => r.Customer)
                .WithMany(c => c.Rentals)
                .HasForeignKey(r => r.CustomerId);
        });

        modelBuilder.Entity<Notify>().ToTable("Notify");
    }
}
