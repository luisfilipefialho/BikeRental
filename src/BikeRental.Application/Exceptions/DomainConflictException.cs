namespace BikeRental.Application.Exceptions;

public class DomainConflictException : Exception
{
    public DomainConflictException(string message)
        : base(message) { }
}
