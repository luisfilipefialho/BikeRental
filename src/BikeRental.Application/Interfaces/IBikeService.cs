using BikeRental.Application.DTOs.Bike;
using BikeRental.Domain.Entities;

namespace BikeRental.Application.Interfaces;

public interface IBikeService
{
    Task CreateAsync(CreateBikeRequest request);
    Task<GetBikeResponse> GetByIdAsync(Guid id);
    Task<IEnumerable<GetBikeResponse>> GetAllAsync(string? plateFilter);
    Task UpdatePlateAsync(Guid id, UpdateBikeRequest request);
    Task DeleteAsync(Guid id);
}
