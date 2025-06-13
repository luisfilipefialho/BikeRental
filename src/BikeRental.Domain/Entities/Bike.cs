namespace BikeRental.Domain.Entities;

public class Bike
{
    public Guid Id { get; set; } // Identificador
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!; // Placa única

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}