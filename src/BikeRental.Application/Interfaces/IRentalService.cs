using BikeRental.Application.DTOs.Rental;

namespace BikeRental.Application.Interfaces;

public interface IRentalService
{
    Task CreateAsync(CreateRentalRequest request);
    Task<GetRentalResponse?> GetByIdAsync(Guid id);
    Task UpdateReturnDateAsync(Guid id, UpdateRentalRequest request);
}
