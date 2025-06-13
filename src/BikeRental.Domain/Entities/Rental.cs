namespace BikeRental.Domain.Entities;

public class Rental
{
    public Rental(Guid guid, Guid id1, Guid id2, DateTime startDate, DateTime endDate, DateTime expectedEndDate, int planDays)
    {
        StartDate = startDate;
        EndDate = endDate;
        ExpectedEndDate = expectedEndDate;
    }

    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BikeId { get; set; }
    public DateTime StartDate { get; set; } // First Day after Creation
    public DateTime EndDate { get; set; }   // Real End
    public DateTime ExpectedEndDate { get; set; } // Expected End
    public decimal TotalCost { get; set; }

    public Customer Customer { get; set; } = null!;
    public Bike Bike { get; set; } = null!;

    public int RentalDays => (EndDate - StartDate).Days;
} 
