using Microsoft.AspNetCore.Mvc.Testing;

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

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            // Read the response content as a string
            var jsonResponseString = await response.Content.ReadAsStringAsync();

            // Parse the JSON response into a JObject
            var jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(jsonResponseString);

            // Assert specific properties or values in the parsed JSON object
            Assert.Equal(gameId, jsonResponse["id"]);
            Assert.Equal("Example Game Name", jsonResponse["gameName"]);
            // Add more assertions as needed
        }
    }
}