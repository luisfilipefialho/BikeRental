using BikeRental.Application.DTOs.Customer;
using BikeRental.Domain.Entities;

namespace BikeRental.Application.Interfaces;

public interface ICustomerService
{
    Task CreateAsync(CreateCustomerRequest request);
    Task UploadCnhAsync(Guid customerId, UploadCnhRequest request);
}
