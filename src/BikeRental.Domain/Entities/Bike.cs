namespace BikeRental.Domain.Entities;

public class Bike
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!; // Unique

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}