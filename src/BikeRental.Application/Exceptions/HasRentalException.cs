namespace BikeRental.Application.Exceptions;

public class HasRentalException : Exception
{
    public HasRentalException(string message)
        : base(message) { }
}
