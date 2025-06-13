namespace BikeRental.Application.DTOs.Rental;

public class GetRentalResponse
{
    public string Identifier { get; set; } = null!;
    public decimal DailyRate { get; set; }
    public string CustomerId { get; set; } = null!;
    public string BikeId { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}