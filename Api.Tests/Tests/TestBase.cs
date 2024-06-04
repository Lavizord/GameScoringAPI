using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace Api.Tests
{
    public class TestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> Factory;
        protected readonly HttpClient Client;
        protected readonly GameDBContext DbContext;

        public TestBase(WebApplicationFactory<Program> factory)
        {
            Factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<GameDBContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add DbContext using in-memory database for testing
                    services.AddDbContext<GameDBContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryGameDb");
                    });
                });
            });

            // Create the HttpClient
            Client = Factory.CreateClient();

            // Get the DbContext instance
            using var scope = Factory.Services.CreateScope();
            DbContext = scope.ServiceProvider.GetRequiredService<GameDBContext>();

            // Ensure the database is created
            DbContext.Database.EnsureCreated();

            //SeedTestData();

        }

        public T GetDto<T>(string name)
        {
            var directory = this.GetType().Name;
            var jsonDto = File.ReadAllText($"./Data/{directory}/{name}.json");
            return JsonSerializer.Deserialize<T>(jsonDto);
        }


        // Seed initial test data
        protected void SeedTestData()
        {
            // Add initial seeding logic here
            DbContext.Games.Add(new Game
            {
                GameName = "ExistingGame",
                GameDescription = "Existing game description."
            });

            DbContext.SaveChanges();
        }


        protected void SeedGamesData()
        {   
            // This will load the ./Data/GamesGetTests/GamesData.Json
            var dto = GetDto<GameWithMatchDataPointDto[]>("GamesData");
            foreach (GameWithMatchDataPointDto game in dto)
            {
                
            }
        }
    }
}