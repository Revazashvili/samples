using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PostService.Data;
using PostService.Entities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ListenForIntegrationEvents();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PostServiceContext>(options =>
    options.UseSqlite(@"Data Source=post.db"));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    var serviceScope = app.Services.CreateAsyncScope();
    var postServiceContext = serviceScope.ServiceProvider.GetRequiredService<PostServiceContext>();
    await postServiceContext.Database.EnsureCreatedAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();


static void ListenForIntegrationEvents()
{
    var factory = new ConnectionFactory();
    using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();
    channel.QueueDeclare("user.postservice", true,autoDelete:false,exclusive:false);
    channel.QueueBind("user.postservice","user","");
    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (model, ea) =>
    {
        var contextOptions = new DbContextOptionsBuilder<PostServiceContext>()
            .UseSqlite(@"Data Source=post.db")
            .Options;
        var dbContext = new PostServiceContext(contextOptions);                
                
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [x] Received {0}", message);

        var data = JObject.Parse(message);
        var type = ea.RoutingKey;
        if (type == "user.add")
        {
            dbContext.Users.Add(new User()
            {
                Id = data["id"].Value<int>(),
                Name = data["name"].Value<string>()
            });
            dbContext.SaveChanges();
        }
        else if (type == "user.update")
        {
            var user = dbContext.Users.First(a => a.Id == data["id"].Value<int>());
            user.Name = data["newName"].Value<string>();
            dbContext.SaveChanges();
        }
    };
    channel.BasicConsume(queue: "user.postservice",
        autoAck: true,
        consumer: consumer);
}