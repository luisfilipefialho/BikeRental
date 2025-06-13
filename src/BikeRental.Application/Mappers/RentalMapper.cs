using BikeRental.Application.DTOs.Rental;
using BikeRental.Domain.Entities;

namespace BikeRental.Application.Mappers;

public static class RentalMapper
{
    public static GetRentalResponse ToResponse(Rental rental)
    {
        return new GetRentalResponse
        {
            Identifier = rental.Id.ToString(),
            DailyRate = rental.TotalCost / Math.Max((rental.EndDate - rental.StartDate).Days, 1),
            CustomerId = rental.CustomerId.ToString(),
            BikeId = rental.BikeId.ToString(),
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            ExpectedEndDate = rental.ExpectedEndDate,
            ReturnDate = rental.EndDate != rental.ExpectedEndDate ? rental.EndDate : null
        };
    }
}
