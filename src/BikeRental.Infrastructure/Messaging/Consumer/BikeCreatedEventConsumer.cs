using BikeRental.Domain.Entities;
using BikeRental.Infrastructure.Data;
using BikeRental.Messaging.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reactive;
using System.Text;
using System.Text.Json;

namespace BikeRental.Infrastructure.Messaging.Consumer;

public class BikeCreatedEventConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;

    public BikeCreatedEventConsumer(IConfiguration config, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:Host"] ?? "localhost",
            Port = 5672,
            UserName = config["RabbitMQ:User"] ?? "guest",
            Password = config["RabbitMQ:Pass"] ?? "guest"
        };

        var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.ExchangeDeclare("bikerental", ExchangeType.Topic);
        _channel.QueueDeclare("bike.created.2024", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind("bike.created.2024", "bikerental", "bike.created");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var bikeEvent = JsonSerializer.Deserialize<BikeCreatedEvent>(json);

            if (bikeEvent is { Year: 2024 })
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await db.Notifications.AddAsync(new Notify
                {
                    Message = $"New 2024 bike created: {bikeEvent.Identifier}",
                    CreatedAt = DateTime.UtcNow
                });

                await db.SaveChangesAsync();
            }
        };

        _channel.BasicConsume("bike.created.2024", true, consumer);
        return Task.CompletedTask;
    }
}
