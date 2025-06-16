using BikeRental.Application.DTOs.Rental;

namespace BikeRental.Application.Interfaces;

public interface IRentalService
{
    Task CreateAsync(CreateRentalRequest request);
    Task<GetRentalResponse?> GetByIdAsync(string identifier);
    Task UpdateReturnDateAsync(string identifier, UpdateRentalRequest request);
}
