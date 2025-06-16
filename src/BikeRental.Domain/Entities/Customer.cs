namespace BikeRental.Domain.Entities;

public class Customer
{
    public Customer() { }
    public Customer(string identifier, string fullName, string cnpj, DateTime birthDate, string cnhNumber, string cnhType)
    {
        Identifier = identifier;
        FullName = fullName;
        Cnpj = cnpj;
        BirthDate = birthDate;
        CnhNumber = cnhNumber;
        CnhType = cnhType;
    }

    public string Identifier { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Cnpj { get; set; } = null!; // Unique
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; } = null!; // Unique
    public string CnhType { get; set; } = null!; // "A", "B", "A+B"
    public string? CnhImagePath { get; set; } // File path on S3

    public bool HasCategoryA => CnhType == "A" || CnhType == "A+B";

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}
