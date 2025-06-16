using BikeRental.Application.Interfaces;
using BikeRental.Domain.Interfaces.Repositories;
using BikeRental.Domain.Interfaces.Storage;
using BikeRental.Infrastructure.Data;
using BikeRental.Infrastructure.Messaging;
using BikeRental.Infrastructure.Messaging.Consumer;
using BikeRental.Infrastructure.Repositories;
using BikeRental.Infrastructure.Storage;
using BikeRental.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeRental.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IBikeRepository, BikeRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IRentalRepository, RentalRepository>();
        services.AddScoped<IStorageService, MinioStorageService>();
        services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();
        services.AddHostedService<BikeCreatedEventConsumer>();


        return services;
    }
}
