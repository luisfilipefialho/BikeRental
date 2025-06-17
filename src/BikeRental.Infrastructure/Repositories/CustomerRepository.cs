using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;
using BikeRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BikeRental.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customer?> GetByIdAsync(string id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<Customer?> GetByCnpjAsync(string cnpj)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Cnpj == cnpj);
    }

    public async Task<Customer?> GetByCnhNumberAsync(string cnhNumber)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.CnhNumber == cnhNumber);
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await _context.Customers.AnyAsync(c => c.Identifier == id);
    }

    public async Task<bool> ExistsAsync(string cnpj, string cnhNumber)
    {
        return await _context.Customers.AnyAsync(c => c.Cnpj == cnpj || c.CnhNumber == cnhNumber);
    }
    public void Remove(Customer customer)
    {
        _context.Customers.Remove(customer);
        _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
    }
}
