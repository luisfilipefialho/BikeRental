namespace BikeRental.Domain.Entities;

public class Notify
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
