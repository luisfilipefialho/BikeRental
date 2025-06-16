using System.Diagnostics;
using BikeRental.Application.DTOs.Bike;
using BikeRental.Application.Exceptions;
using BikeRental.Application.Interfaces;
using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;
using BikeRental.Messaging;
using BikeRental.Messaging.Events;

namespace BikeRental.Application.Services;

public class BikeService : IBikeService
{
    private readonly IBikeRepository _bikeRepository;
    private readonly IEventPublisher _eventPublisher;

    public BikeService(IBikeRepository bikeRepository, IEventPublisher eventPublisher)
    {
        _bikeRepository = bikeRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task CreateAsync(CreateBikeRequest request)
    {
        var existId = await _bikeRepository.GetByIdAsync(request.Identifier);
        if (existId != null) throw new DomainConflictException("Bike with this Id already exists");

        var exists = await _bikeRepository.ExistsByPlateAsync(request.LicensePlate);
        if (exists) throw new DomainConflictException("Bike with this Plate already exists");

        var bike = new Bike(request.Identifier, request.Model, request.Year, request.LicensePlate);

        await _bikeRepository.AddAsync(bike);

        await _eventPublisher.PublishAsync(new BikeCreatedEvent
        {
            Identifier = bike.Identifier,
            Year = bike.Year,
            Model = bike.Model,
            LicensePlate = bike.LicensePlate
        }, "bike.created");
    }

    public async Task<GetBikeResponse?> GetByIdAsync(string id)
    {
        var bike = await _bikeRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("Bike not found");

        return new GetBikeResponse(bike.Identifier, bike.Year, bike.Model, bike.LicensePlate);
    }

    public async Task<IEnumerable<GetBikeResponse>> GetAllAsync(string? plateFilter)
    {
        var bikes = await _bikeRepository.GetAllAsync(plateFilter);
        return bikes.Select(b => new GetBikeResponse(b.Identifier, b.Year, b.Model, b.LicensePlate));
    }

    public async Task UpdatePlateAsync(string id, UpdateBikeRequest request)
    {
        var exists = await _bikeRepository.ExistsByPlateAsync(request.LicensePlate);
        if (exists) throw new DomainConflictException("Bike with this plate already exists");

        var bike = await _bikeRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("Bike not found");

        bike.LicensePlate = request.LicensePlate;
        await _bikeRepository.UpdateAsync(bike);
    }

    public async Task DeleteAsync(string identifier)
    {
        var bike = await _bikeRepository.GetByIdAsync(identifier) ?? throw new EntityNotFoundException("Bike Not Found");
        if (bike.Rentals.Count != 0)
            throw new HasRentalException("Bike has rentals");

        _bikeRepository.Remove(bike);
    }
}
