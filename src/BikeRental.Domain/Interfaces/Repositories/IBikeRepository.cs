using BikeRental.Domain.Entities;

namespace BikeRental.Domain.Interfaces.Repositories
{
    public interface IBikeRepository : IBaseRepository<Bike>
    {
        Task<Bike?> GetByLicensePlateAsync(string licensePlate);
        Task<bool> HasRentalAsync(Guid bikeId);
    }
}
