namespace BikeRental.Application.DTOs.Rental;

public class CreateRentalRequest
{
    public Guid CustomerId { get; set; }
    public Guid BikeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public int PlanDays { get; set; } // 7, 15, 30, 45, or 50
}