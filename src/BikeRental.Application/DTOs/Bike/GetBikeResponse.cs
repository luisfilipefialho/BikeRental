namespace BikeRental.Application.DTOs.Bike;

public class GetBikeResponse
{
    public GetBikeResponse(string identifier, int year, string model, string licensePlate)
    {
        Identifier = identifier;
        Year = year;
        Model = model;
        LicensePlate = licensePlate;
    }

    public string Identifier { get; } = null!;
    public int Year { get; set; }
    public string Model { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
}