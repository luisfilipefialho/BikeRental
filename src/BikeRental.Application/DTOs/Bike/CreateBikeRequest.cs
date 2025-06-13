namespace BikeRental.Application.DTOs.Bike;

public class CreateBikeRequest
{
    public string Identifier { get; set; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
}