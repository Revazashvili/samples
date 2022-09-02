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
    var userServiceContext = app.Services.GetService<UserServiceContext>()!;
    await userServiceContext.Database.EnsureCreatedAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();
