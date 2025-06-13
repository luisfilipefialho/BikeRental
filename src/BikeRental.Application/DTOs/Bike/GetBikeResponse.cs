namespace BikeRental.Application.DTOs.Bike;

public class GetBikeResponse
{
    public string Id { get; set; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
}