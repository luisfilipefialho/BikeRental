namespace BikeRental.Application.Exceptions;

public class ForbiddenOperationException : Exception
{
    public ForbiddenOperationException(string message)
        : base(message) { }
}
