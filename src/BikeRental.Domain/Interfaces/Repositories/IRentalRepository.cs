using BikeRental.Domain.Entities;

namespace BikeRental.Domain.Interfaces.Repositories
{
    public interface IRentalRepository : IBaseRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetByCustomerIdAsync(string customerId);
        Task<IEnumerable<Rental>> GetByBikeIdAsync(string bikeId);
    }
}
