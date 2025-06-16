namespace BikeRental.Application.DTOs.Rental;

public class CreateRentalRequest
{
    public string CustomerId { get; set; } = null!;
    public string BikeId { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public int PlanDays { get; set; } // 7, 15, 30, 45, or 50
}