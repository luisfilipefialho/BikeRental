namespace BikeRental.Application.DTOs.Customer;

public class CreateCustomerRequest
{
    public string Identifier { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; } = null!;
    public string CnhType { get; set; } = null!; // Expected values: "A", "B", "A+B"
    public string CnhImageBase64 { get; set; } = null!;
}