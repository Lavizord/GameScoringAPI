using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            SeedGamesData();
        }   

        private JsonSerializerOptions GetOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };
        }

        public List<T> GetDto<T>(string name)
        {
            var directory = this.GetType().Name;
            var jsonDto = File.ReadAllText($"./Data/{directory}/{name}.json");
            return JsonSerializer.Deserialize<List<T>>(jsonDto, GetOptions());
        }

        protected void SeedGamesData()
        {
            // This will load the ./Data/GamesGetTests/GamesData.Json
            var dto = GetDto<GameDto>("GamesData");
            foreach (GameDto game in dto)
            {
                DbContext.Games.Add(new Game
                {
                    GameName = game.GameName,
                    GameDescription = game.GameDescription,
                    MinPlayers = game.MinPlayers,
                    MaxPlayers = game.MaxPlayers,
                    AverageDuration = game.AverageDuration,
                    MatchesCount = game.MatchesCount
                });
            }
            DbContext.SaveChanges();
        }
    }
}