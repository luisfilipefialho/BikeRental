using BikeRental.Domain.Entities;

namespace BikeRental.Domain.Interfaces.Repositories
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer?> GetByCnpjAsync(string cnpj);
        Task<Customer?> GetByCnhNumberAsync(string cnhNumber);
    }
}
