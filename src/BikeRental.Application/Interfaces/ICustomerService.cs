using BikeRental.Application.DTOs.Customer;
using BikeRental.Domain.Entities;

namespace BikeRental.Application.Interfaces;

public interface ICustomerService
{
    Task CreateAsync(CreateCustomerRequest request);
    Task UploadCnhAsync(string customerId, UploadCnhRequest request);
}
