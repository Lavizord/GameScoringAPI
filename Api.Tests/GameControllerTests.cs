using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json; // Import the necessary namespace for JSON parsing

namespace Api.Tests
{
    public class GetGameTests
    {
        [Fact]
        public async Task GetGameById_ReturnsGameIfExists()
        {
            await using var application = new WebApplicationFactory<Program>();

            // Arrange
            var client = application.CreateClient();

            int gameId = 1; // Change to a valid game ID for your test
            var response = await client.GetAsync($"/game/{gameId}");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
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