using BikeRental.Domain.Interfaces.Storage;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace BikeRental.Infrastructure.Storage;

public class MinioStorageService : IStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;

    public MinioStorageService(IConfiguration config)
    {
        _bucketName = config["Minio:Bucket"] ?? "cnhs";
        _minioClient = new MinioClient()
            .WithEndpoint(config["Minio:Endpoint"] ?? "localhost:9000")
            .WithCredentials(config["Minio:AccessKey"], config["Minio:SecretKey"])
            .Build();
    }

    public async Task<string> UploadAsync(string objectName, Stream content, string contentType)
    {
        var beArgs = new BucketExistsArgs().WithBucket(_bucketName);
        bool found = await _minioClient.BucketExistsAsync(beArgs);
        if (!found)
        {
            var mbArgs = new MakeBucketArgs().WithBucket(_bucketName);
            await _minioClient.MakeBucketAsync(mbArgs);
        }

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectName)
            .WithStreamData(content)
            .WithObjectSize(content.Length)
            .WithContentType(contentType);

        await _minioClient.PutObjectAsync(putObjectArgs);

        return $"{_bucketName}/{objectName}";
    }
}
