using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.DocumentFilter<SwaggerTagsSorter>();
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "https://lively-bay-05f413403.5.azurestaticapps.net")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Construct the database path
var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "games.db");

builder.Services.AddDbContext<GameDBContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

Console.WriteLine($"Checking Database Connection:");
Console.WriteLine($"Database File Path: {dbPath}");
Console.WriteLine($"Database File Exists: {File.Exists(dbPath)}");


var app = builder.Build();


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var dbContext = services.GetRequiredService<GameDBContext>();
//    dbContext.Database.Migrate();
//}


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<GameDBContext>();
    
    // Delete the existing database
    dbContext.Database.EnsureDeleted();

    // Create a new database based on the model definitions
    dbContext.Database.EnsureCreated();
}


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game API V0.141");
    c.RoutePrefix = string.Empty; // Makes Swagger UI available at the app's root
});


app.UseHttpsRedirection();

// Use CORS with the specified policy
app.UseCors("AllowSpecificOrigins");

// Map Game Endpoints
app.MapGetGameEndpoints();
app.MapGameEndpoints();

// Map Match endpoints
app.MapMatchEndpoints();

// Map the MatchDataPoints endpoints
app.MapMatchDataPointEndpoints();

// Test endpoints
app.MapTestEndpoints();

app.Run();