using BikeRental.Application.DTOs.Rental;
using BikeRental.Application.Interfaces;
using BikeRental.Application.Mappers;
using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;

namespace BikeRental.Application.Services;

public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBikeRepository _bikeRepository;

    public RentalService(
        IRentalRepository rentalRepository,
        ICustomerRepository customerRepository,
        IBikeRepository bikeRepository)
    {
        _rentalRepository = rentalRepository;
        _customerRepository = customerRepository;
        _bikeRepository = bikeRepository;
    }

    public async Task CreateAsync(CreateRentalRequest request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId)
            ?? throw new Exception("Customer not found");

        var bike = await _bikeRepository.GetByIdAsync(request.BikeId)
            ?? throw new Exception("Bike not found");

        if (!customer.HasCategoryA)
            throw new Exception("Customer is not allowed to rent");

        var startDate = request.StartDate.Date;
        var endDate = request.EndDate.Date;
        var expectedEndDate = request.ExpectedEndDate.Date;

        if (startDate <= DateTime.UtcNow.Date)
            throw new Exception("Start date must be at least one day after today");

        decimal dailyRate = request.PlanDays switch
        {
            7 => 30m,
            15 => 28m,
            30 => 22m,
            45 => 20m,
            50 => 18m,
            _ => throw new Exception("Invalid plan")
        };

        var totalCost = dailyRate * request.PlanDays;

        var rental = new Rental(Guid.NewGuid(), customer.Id, bike.Id, startDate, endDate, expectedEndDate, request.PlanDays)
        {
            TotalCost = totalCost
        };

        await _rentalRepository.AddAsync(rental);
    }

    public async Task<GetRentalResponse?> GetByIdAsync(Guid id)
    {
        var rental = await _rentalRepository.GetByIdAsync(id);
        if (rental == null)
            return null;
        return RentalMapper.ToResponse(rental);
    }

    public async Task UpdateReturnDateAsync(Guid id, UpdateRentalRequest request)
    {
        var rental = await _rentalRepository.GetByIdAsync(id) ?? throw new Exception("Rental not found");

        var returnDate = request.ReturnDate.Date;
        rental.EndDate = returnDate;

        if (returnDate < rental.ExpectedEndDate.Date)
        {
            var unusedDays = (rental.ExpectedEndDate.Date - returnDate).Days;
            var multaPercentual = rental.RentalDays switch
            {
                7 => 0.20m,
                15 => 0.40m,
                _ => 0m
            };

            var valorMulta = (rental.TotalCost / rental.RentalDays) * unusedDays * multaPercentual;
            rental.TotalCost += valorMulta;
        }
        else if (returnDate > rental.ExpectedEndDate.Date)
        {
            var extraDays = (returnDate - rental.ExpectedEndDate.Date).Days;
            rental.TotalCost += extraDays * 50m;
        }

        _rentalRepository.Update(rental);
    }
}
