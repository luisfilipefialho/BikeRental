namespace BikeRental.Domain.Entities;

public class Bike
{
    public Guid Id { get; set; }
    public string Identifier { get; set; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!; // Unique

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
    public Guid Guid { get; }
    public Bike(Guid guid, string identifier, string model, int year, string licensePlate)
    {
        Guid = guid;
        Identifier = identifier;
        Model = model;
        Year = year;
        LicensePlate = licensePlate;
    }
}
