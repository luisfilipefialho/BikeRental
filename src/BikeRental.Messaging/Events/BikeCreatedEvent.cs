﻿namespace BikeRental.Messaging.Events;

public class BikeCreatedEvent
{
    public string Identifier { get; set; } = null!;
    public string LicensePlate { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int Year { get; set; }
}
