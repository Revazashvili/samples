using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.Integration;

IConsumer consumer = new Consumer();
consumer.Consume();
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