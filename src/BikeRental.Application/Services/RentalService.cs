using BikeRental.Application.DTOs.Rental;
using BikeRental.Application.Exceptions;
using BikeRental.Application.Interfaces;
using BikeRental.Domain.Entities;
using BikeRental.Domain.Interfaces.Repositories;

namespace BikeRental.Application.Services;

public class RentalService(
    IRentalRepository rentalRepository,
    ICustomerRepository customerRepository,
    IBikeRepository bikeRepository) : IRentalService
{
    public async Task CreateAsync(CreateRentalRequest request)
    {
        var customer = await customerRepository.GetByIdAsync(request.CustomerId)
            ?? throw new EntityNotFoundException("Customer not found");

        var bike = await bikeRepository.GetByIdAsync(request.BikeId)
            ?? throw new EntityNotFoundException("Bike not found");

        if (!customer.HasCategoryA)
            throw new ForbiddenOperationException("Customer is not allowed to rent");

        var startDate = request.StartDate.Date;
        var endDate = request.EndDate.Date;
        var expectedEndDate = request.ExpectedEndDate.Date;

        if (startDate <= DateTime.UtcNow.Date)
            throw new InvalidInputException("Start date must be at least one day after today");

        decimal dailyRate = request.PlanDays switch
        {
            7 => 30m,
            15 => 28m,
            30 => 22m,
            45 => 20m,
            50 => 18m,
            _ => throw new InvalidInputException("Invalid plan")
        };

        var totalCost = dailyRate * request.PlanDays;

        var rental = new Rental(Guid.NewGuid() , customer.Identifier, bike.Identifier, startDate, endDate, expectedEndDate)
        {
            TotalCost = totalCost
        };

        await rentalRepository.AddAsync(rental);
    }

    public async Task<GetRentalResponse?> GetByIdAsync(string id)
    {
        var rental = await rentalRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("Rental not found");
        return new GetRentalResponse
        {
            Identifier = rental.Identifier,
            DailyRate = rental.TotalCost / Math.Max((rental.EndDate - rental.StartDate).Days, 1),
            CustomerId = rental.CustomerId,
            BikeId = rental.BikeId,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            ExpectedEndDate = rental.ExpectedEndDate,
            ReturnDate = rental.EndDate != rental.ExpectedEndDate ? rental.EndDate : null
        };
    }

    public async Task UpdateReturnDateAsync(string id, UpdateRentalRequest request)
    {
        var rental = await rentalRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("Rental not found"); 

        var returnDate = request.ReturnDate.Date;
        rental.EndDate = returnDate;

        if (returnDate < rental.StartDate.Date)
            throw new InvalidInputException("Return date cannot be before start date");


        if (returnDate < rental.ExpectedEndDate.Date)
        {
            var unusedDays = (rental.ExpectedEndDate.Date - returnDate).Days;
            var finePercent = rental.RentalDays switch
            {
                7 => 0.20m,
                15 => 0.40m,
                _ => 0m
            };

            var fineValue = rental.TotalCost / rental.RentalDays * unusedDays * finePercent;
            rental.TotalCost += fineValue;
        }
        else if (returnDate > rental.ExpectedEndDate.Date)
        {
            var extraDays = (returnDate - rental.ExpectedEndDate.Date).Days;
            rental.TotalCost += extraDays * 50m;
        }

        await rentalRepository.UpdateAsync(rental);
    }
}
