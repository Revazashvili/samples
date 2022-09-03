using RabbitMQ.Client;
using UserService.Extensions;

namespace UserService.Integration;

public class Publisher : IPublisher
{
    public void PublishToMessageQueue(string exchange,string integrationEvent, object data)
    {
        var factory = new ConnectionFactory();
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.ExchangeDeclare(exchange,"fanout",true);
        channel.BasicPublish(exchange: exchange,
            routingKey: integrationEvent,
            basicProperties: null,
            body: data.ToBytes());
    }
}