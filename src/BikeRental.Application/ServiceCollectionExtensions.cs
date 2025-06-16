using BikeRental.Application.Interfaces;
using BikeRental.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BikeRental.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBikeService, BikeService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IRentalService, RentalService>();

        return services;
    }
}
