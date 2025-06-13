using BikeRental.Application.DTOs.Bike;
using BikeRental.Application.Interfaces;
using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;
//using BikeRental.Domain.Interfaces.EventHandler;

namespace BikeRental.Application.Services;

public class BikeService : IBikeService
{
    private readonly IBikeRepository _bikeRepository;
    //private readonly IEventPublisher _eventPublisher;

    public BikeService(IBikeRepository bikeRepository) // , IEventPublisher eventPublisher
    {
        _bikeRepository = bikeRepository;
        //_eventPublisher = eventPublisher;
    }

    public async Task CreateAsync(CreateBikeRequest request)
    {
        var exists = await _bikeRepository.ExistsByPlateAsync(request.LicensePlate);
        if (exists) throw new Exception("Bike with same plate already exists.");

        var bike = new Bike(Guid.NewGuid(), request.Identifier, request.Model, request.Year, request.LicensePlate);

        await _bikeRepository.AddAsync(bike);

        //await _eventPublisher.PublishAsync(new { bike.Id, bike.Year, bike.Model }, "bike.created");
    }

    public async Task<GetBikeResponse> GetByIdAsync(Guid id)
    {
        var bike = await _bikeRepository.GetByIdAsync(id) ?? throw new Exception("Bike not found");

        return new GetBikeResponse(bike.Identifier, bike.Year, bike.Model, bike.LicensePlate);
    }

    public async Task<IEnumerable<GetBikeResponse>> GetAllAsync(string? plateFilter)
    {
        var bikes = await _bikeRepository.GetAllAsync(plateFilter);
        return bikes.Select(b => new GetBikeResponse(b.Identifier, b.Year, b.Model, b.LicensePlate));
    }

    public async Task UpdatePlateAsync(Guid id, UpdateBikeRequest request)
    {
        var exists = await _bikeRepository.ExistsByPlateAsync(request.LicensePlate);
        if (exists) throw new Exception("Bike with same plate already exists.");

        var bike = await _bikeRepository.GetByIdAsync(id) ?? throw new Exception("Bike not found");
        bike.LicensePlate = request.LicensePlate;
        _bikeRepository.Update(bike);
    }

    public async Task DeleteAsync(Guid id)
    {
        var bike = await _bikeRepository.GetByIdAsync(id) ?? throw new Exception("Bike not found");
        if (bike.Rentals.Any()) throw new Exception("Bike has rentals and cannot be deleted");
        _bikeRepository.Remove(bike);
    }
}