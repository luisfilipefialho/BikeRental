namespace BikeRental.Domain.Entities;

public class Rental
{
    public Rental() { }
    public Rental(Guid id, string customerId, string bikeId, DateTime startDate, DateTime endDate, DateTime expectedEndDate)
    {
        Identifier = id.ToString();
        CustomerId = customerId;
        BikeId = bikeId;
        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
    }

    public string Identifier { get; set; } = null!;
    public string CustomerId { get; set; } = null!;
    public string BikeId { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public decimal TotalCost { get; set; }

    public Customer Customer { get; set; } = null!;
    public Bike Bike { get; set; } = null!;

    public int RentalDays => (EndDate - StartDate).Days;
}
