using BikeRental.Domain.Entities;

namespace BikeRental.Domain.Interfaces.Repositories
{
    public interface IBikeRepository : IBaseRepository<Bike>
    {
        Task<IEnumerable<Bike>> GetAllAsync(string? licensePlate);
        Task<Bike?> GetByLicensePlateAsync(string licensePlate);
        Task<bool> ExistsByPlateAsync(string plate);
        Task<bool> HasRentalAsync(string bikeIdentifier);
    }
}
