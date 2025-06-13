using BikeRental.Application.DTOs.Customer;
using BikeRental.Application.Interfaces;
using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;
//using BikeRental.Domain.Interfaces.Storage;

namespace BikeRental.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    //private readonly IStorageService _storageService;

    public CustomerService(ICustomerRepository customerRepository) //, IStorageService storageService
    {
        _customerRepository = customerRepository;
        //_storageService = storageService;
    }

    public async Task CreateAsync(CreateCustomerRequest request)
    {
        var exists = await _customerRepository.ExistsAsync(request.Cnpj, request.CnhNumber);
        if (exists) throw new Exception("Customer already exists with same CNPJ or CNH");

        var customer = new Customer(
            Guid.NewGuid(),
            request.FullName,
            request.Cnpj,
            request.BirthDate,
            request.CnhNumber,
            request.CnhType
        );

        if (!string.IsNullOrEmpty(request.CnhImageBase64))
        {
            //var imageName = await _storageService.UploadBase64ImageAsync(customer.Id.ToString(), request.CnhImageBase64);
            //customer.CnhImageFileName = imageName;
        }

        await _customerRepository.AddAsync(customer);
    }

    public async Task UploadCnhAsync(Guid customerId, UploadCnhRequest request)
    {
        //var customer = await _customerRepository.GetByIdAsync(customerId) ?? throw new Exception("Customer not found");
        //var imageName = await _storageService.UploadBase64ImageAsync(customer.Id.ToString(), request.CnhImageBase64);
        //customer.SetCnhImage(imageName);
        //await _customerRepository.UpdateAsync(customer);
    }
}