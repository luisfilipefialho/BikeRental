namespace BikeRental.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Cnpj { get; set; } = null!; // Unique
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; } = null!; // Unique
    public string CnhType { get; set; } = null!; // "A", "B", "A+B"
    public string? CnhImageFileName { get; set; } // File name on s3

    public bool HasCategoryA => CnhType == "A" || CnhType == "A+B";

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
