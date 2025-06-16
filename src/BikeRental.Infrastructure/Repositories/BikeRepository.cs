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

    public async Task<IEnumerable<Bike>> GetAllAsync()
    {
        return await _context.Bikes.ToListAsync();
    }

    public async Task<IEnumerable<Bike>> GetAllAsync(string? licensePlate)
    {
        if (string.IsNullOrEmpty(licensePlate))
        {
            return await _context.Bikes.ToListAsync();
        }

        return await _context.Bikes
            .Where(b => b.LicensePlate.Contains(licensePlate))
            .ToListAsync();
    }

    public async Task<Bike?> GetByIdAsync(string identifier)
    {
        return await _context.Bikes.FindAsync(identifier);
    }

    public async Task<Bike?> GetByLicensePlateAsync(string licensePlate)
    {
        return await _context.Bikes
            .FirstOrDefaultAsync(b => b.LicensePlate == licensePlate);
    }

    public async Task<bool> ExistsAsync(string identifier)
    {
        return await _context.Bikes.AnyAsync(b => b.Identifier == identifier);
    }

    public async Task<bool> ExistsByPlateAsync(string plate)
    {
        return await _context.Bikes.AnyAsync(b => b.LicensePlate == plate);
    }

    public async Task<bool> HasRentalAsync(string bikeId)
    {
        return await _context.Rentals.AnyAsync(r => r.BikeId == bikeId);
    }

    public void Remove(Bike bike)
    {
        _context.Bikes.Remove(bike);
        _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Bike bike)
    {
        _context.Bikes.Update(bike);
        await _context.SaveChangesAsync();
    }
}