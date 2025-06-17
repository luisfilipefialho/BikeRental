namespace BikeRental.Domain.Entities;

public class Bike
{
    public string Identifier { get; set; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public Bike() { }

    public Bike(string identifier, string model, int year, string licensePlate)
    {
        Identifier = identifier;
        Model = model;
        Year = year;
        LicensePlate = licensePlate;
    }
}
