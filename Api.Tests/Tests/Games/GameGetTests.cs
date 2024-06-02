using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Api.Tests
{
    public class GameGetTests :TestBase
    {
        public GameGetTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }
        
        [Fact]
        public async Task GetGameById_ReturnsGameIfExists()
        {
            int gameId = 1; // Change to a valid game ID for your test
            var response = await Client.GetAsync($"/game/{gameId}");
            
            SeedGamesData();

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            // Add more assertions to verify the content of the response
            
            /*
            // Parse the JSON response into a JObject
            var jsonResponse = JObject.Parse(response.Content.ToString());

            // Assert specific properties or values in the parsed JSON object
            Assert.Equal(gameId, jsonResponse["id"].Value<int>());
            Assert.Equal("Example Game Name", jsonResponse["name"].Value<string>());
            // Add more assertions as needed
            */
        }
    }
}