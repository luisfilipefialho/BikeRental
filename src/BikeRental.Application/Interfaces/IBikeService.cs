using BikeRental.Application.DTOs.Bike;
using BikeRental.Domain.Entities;

namespace BikeRental.Application.Interfaces;

public interface IBikeService
{
    Task CreateAsync(CreateBikeRequest request);
    Task<GetBikeResponse?> GetByIdAsync(string identifier);
    Task<IEnumerable<GetBikeResponse>> GetAllAsync(string? plateFilter);
    Task UpdatePlateAsync(string identifier, UpdateBikeRequest request);
    Task DeleteAsync(string identifier);
}
