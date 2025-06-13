using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;
using BikeRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BikeRental.Infrastructure.Repositories;

public class BikeRepository : IBikeRepository
{
    private readonly AppDbContext _context;

    public BikeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Bike bike)
    {
        await _context.Bikes.AddAsync(bike);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Bike bike)
    {
        _context.Bikes.Remove(bike);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Bike>> GetAllAsync(string? licensePlate)
        => await _context.Bikes.ToListAsync();

    public async Task<IEnumerable<Bike>> FilterByLicensePlateAsync(string plate)
        => await _context.Bikes.Where(b => b.LicensePlate.Contains(plate)).ToListAsync();

    public async Task<Bike?> GetByIdAsync(Guid id)
        => await _context.Bikes.FindAsync(id);

    public async Task<Bike?> GetByLicensePlateAsync(string licensePlate)
        => await _context.Bikes.FirstOrDefaultAsync(b => b.LicensePlate == licensePlate);

    public async Task UpdateAsync(Bike bike)
    {
        _context.Bikes.Update(bike);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> HasRentalAsync(Guid bikeId)
        => await _context.Rentals.AnyAsync(r => r.BikeId == bikeId);

    public async Task<bool> ExistsByPlateAsync(string plate)
        => await _context.Bikes.AnyAsync(b => b.LicensePlate == plate);
}