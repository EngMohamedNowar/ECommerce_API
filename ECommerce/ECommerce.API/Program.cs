using ECommerce.API;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Persistence.Seeding;
using ECommerce.UseCases;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPresentaion();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


var app = builder.Build();

// Configure the HTTP request pipeline.

await using var scope = app.Services.CreateAsyncScope();
var dbSeed = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

await dbSeed.SeedAll();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
