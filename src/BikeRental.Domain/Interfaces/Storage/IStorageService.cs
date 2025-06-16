namespace BikeRental.Domain.Interfaces.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(string objectName, Stream content, string contentType);
}

