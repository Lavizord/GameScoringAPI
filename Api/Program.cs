using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.DocumentFilter<OrderTagsDocumentFilter>();
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


//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
//    });


// Construct the database path
var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "games.db");

builder.Services.AddDbContext<GameDBContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

Console.WriteLine($"Checking Database Connection:");
Console.WriteLine($"Database File Path: {dbPath}");
Console.WriteLine($"Database File Exists: {File.Exists(dbPath)}");


var app = builder.Build();

// Database stuff.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameDBContext>();

    // Delete the existing database
    //dbContext.Database.EnsureDeleted();

    // Create a new database based on the model definitions
    if(!File.Exists(dbPath))
        dbContext.Database.EnsureCreated();

    //Run the migrtions, if any.
    //dbContext.Database.Migrate();

    // This is to make sure the database context supports raw SQL.
    // Since we have unit tests setup, and we change the database to be in memory during the tests,
    // rawsql cant be executed on them.
    if (dbContext.Database.IsRelational())
    {
        // Execute the SQL to create triggers
        dbContext.Database.ExecuteSqlRaw(SqlTriggers.MatchesTriggerInsert);
        dbContext.Database.ExecuteSqlRaw(SqlTriggers.MatchesTriggerDelete);
        dbContext.Database.ExecuteSqlRaw(SqlTriggers.MatchesDataPointTriggerDelete);
        dbContext.Database.ExecuteSqlRaw(SqlTriggers.MatchesDataPointTriggerInsert);
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game API V0.141");
    c.ConfigObject.AdditionalItems.Add("tagsSorter", "alpha");
    c.RoutePrefix = string.Empty; // Makes Swagger UI available at the app's root
});


app.UseHttpsRedirection();

// Use CORS with the specified policy
app.UseCors("AllowSpecificOrigins");

// Map Game Endpoints
app.MapGameEndpoints();

// Map Match endpoints
app.MapMatchEndpoints();

// Map the DataPoints endpoints
app.MapMatchDataPointEndpoints();

// Test endpoints
app.MapTestEndpoints();

app.Run();

public partial class Program { }