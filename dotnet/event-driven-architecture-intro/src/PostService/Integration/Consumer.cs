using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.Entities;
using PostService.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PostService.Integration;

public class Consumer : IConsumer
{
    private const string QueueName = "user.postservice";
    private const string ExchangeName = "user";
    private const string RoutingKey = "";

    private const string UserAddRoutingKey = "user.add";
    private const string UserUpdateRoutingKey = "user.update";
    private PostServiceContext Database()
    {
        var contextOptions = new DbContextOptionsBuilder<PostServiceContext>()
            .UseSqlite(@"Data Source=post.db")
            .Options;
        var dbContext = new PostServiceContext(contextOptions);
        return dbContext;
    }
    public void Consume()
    {
        var factory = new ConnectionFactory();
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(QueueName, true,autoDelete:false,exclusive:false);
        channel.QueueBind(QueueName,ExchangeName,RoutingKey);
        var consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var database = Database();
            var type = ea.RoutingKey!;
            if (type == UserAddRoutingKey)
                AddUser(ea, database);
            else if (type == UserUpdateRoutingKey)
                UpdateUser(ea, database);
            else
                throw new Exception("unknown routing key");
        };
        channel.BasicConsume(queue: QueueName,
            autoAck: true,
            consumer: consumer);
    }


    private void UpdateUser(BasicDeliverEventArgs ea, PostServiceContext database)
    {
        var user = ea.Body.ToModel<UserUpdateIntegrationModel>()!;
        var existingUser = database.Users.First(a => a.Id == user.Id);
        existingUser.Name = user.NewName;
        database.SaveChanges();
    }

    private void AddUser(BasicDeliverEventArgs ea, PostServiceContext database)
    {
        var user = ea.Body.ToModel<UserAddIntegrationModel>()!;
        database.Users.Add(new User()
        {
            Id = user.Id,
            Name = user.Name
        });
        database.SaveChanges();
    }
}