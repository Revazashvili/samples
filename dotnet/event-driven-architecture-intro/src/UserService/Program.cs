using Microsoft.EntityFrameworkCore;
using UserService.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserServiceContext>(options =>
    options.UseSqlite(@"Data Source=user.db"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var serviceScope = app.Services.CreateAsyncScope();
    var userServiceContext = serviceScope.ServiceProvider.GetRequiredService<UserServiceContext>();
    await userServiceContext.Database.EnsureCreatedAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();
