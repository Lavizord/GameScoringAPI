using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Construct the database path
var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "games.db");

builder.Services.AddDbContext<GameDBContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

Console.WriteLine($"Checking Database Connection:");
Console.WriteLine($"Database File Path: {dbPath}");
Console.WriteLine($"Database File Exists: {File.Exists(dbPath)}");


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game API V1");
    c.RoutePrefix = string.Empty; // Makes Swagger UI available at the app's root
});

app.UseHttpsRedirection();



app.MapGameEndpoints();
app.MapMatchEndpoints();
app.MapMatchDataPointEndpoints();
app.Run();