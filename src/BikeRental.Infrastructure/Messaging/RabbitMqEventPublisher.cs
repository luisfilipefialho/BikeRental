using BikeRental.Messaging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BikeRental.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IModel _channel;

    public RabbitMqEventPublisher(IConfiguration config)
    {
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
    }

    public Task PublishAsync<T>(T @event, string routingKey) where T : class
    {
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
        _channel.BasicPublish("bikerental", routingKey, null, body);
        return Task.CompletedTask;
    }
}
