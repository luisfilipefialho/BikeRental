using BikeRental.Domain.Entities;

namespace BikeRental.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string identifier);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> ExistsAsync(string identifier);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        void Remove(T entity);
    }
}
