using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;
using BikeRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BikeRental.Infrastructure.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Rental rental)
    {
        await _context.Rentals.AddAsync(rental);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Rental>> GetAllAsync()
    {
        return await _context.Rentals.ToListAsync();
    }

    public async Task<Rental?> GetByIdAsync(string id)
    {
        return await _context.Rentals.FindAsync(id);
    }

    public async Task<IEnumerable<Rental>> GetByBikeIdAsync(string bikeId)
    {
        return await _context.Rentals
            .Where(r => r.BikeId == bikeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Rental>> GetByCustomerIdAsync(string customerId)
    {
        return await _context.Rentals
            .Where(r => r.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Rentals.AnyAsync(r => r.Identifier == id);
    }

    public void Remove(Rental rental)
    {
        _context.Rentals.Remove(rental);
        _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Rental rental)
    {
        _context.Rentals.Update(rental);
        await _context.SaveChangesAsync();
    }
}