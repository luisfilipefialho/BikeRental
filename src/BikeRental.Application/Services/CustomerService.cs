using BikeRental.Application.DTOs.Customer;
using BikeRental.Application.Exceptions;
using BikeRental.Application.Interfaces;
using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;
using BikeRental.Domain.Interfaces.Storage;

namespace BikeRental.Application.Services;

public class CustomerService(ICustomerRepository customerRepository, IStorageService storageService) : ICustomerService
{
  public async Task CreateAsync(CreateCustomerRequest request)
    {
        var exists = await customerRepository.ExistsAsync(request.Cnpj, request.CnhNumber);
        if (exists) throw new DomainConflictException("Customer with this CNH or CNPJ already exists");

        var customer = new Customer(
            request.Identifier,
            request.FullName,
            request.Cnpj,
            request.BirthDate,
            request.CnhNumber,
            request.CnhType
        );
        await customerRepository.AddAsync(customer);
    }

    public async Task UploadCnhAsync(string customerId, UploadCnhRequest request)
    {
        var customer = await customerRepository.GetByIdAsync(customerId) ?? throw new EntityNotFoundException("Customer not found");

        var base64Span = request.CnhImageBase64.AsSpan();

        string contentType;
        if (base64Span.StartsWith("iVBOR".AsSpan()))
            contentType = "image/png";
        else if (base64Span.StartsWith("Qk".AsSpan()))
            contentType = "image/bmp";
        else
            throw new InvalidInputException("Invalid image (only PNG or BMP)");

        byte[] imageBytes = Convert.FromBase64String(request.CnhImageBase64);
        using var imageStream = new MemoryStream(imageBytes);

        string objectName = $"cnhs/{customerId}.{(contentType == "image/png" ? "png" : "bmp")}";

        string imagePath = await storageService.UploadAsync(objectName, imageStream, contentType);

        customer.CnhImagePath = imagePath;
        await customerRepository.UpdateAsync(customer);
    }
}
