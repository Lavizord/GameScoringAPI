using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

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

            // Read the response content as a string
            var jsonResponseString = await response.Content.ReadAsStringAsync();
            // Parse the JSON response into a JObject
            var jsonResponse = Newtonsoft.Json.Linq.JObject.Parse(jsonResponseString);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            // Assert specific properties or values in the parsed JSON object
            Assert.Equal(gameId, jsonResponse["id"]);
            Assert.Equal("Example Game Name", jsonResponse["gameName"]);
        }

        [Fact]
        public async Task GetGameById_ReturnsNotFound()
        {
            int gameId = -1; // Change to a valid game ID for your test
            var response = await Client.GetAsync($"/game/{gameId}");

            // Read the response content as a string
            var jsonResponseString = await response.Content.ReadAsStringAsync();
            
            // Assert not found.
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);         
            //TODO: Check Error message?   
        }

        [Fact]
        public async Task GetGamesById_ReturnsGameIfExists()
        {
            int requestedGameId = 1; // Change to a valid game ID for your test
            var response = await Client.GetAsync($"/games?id={requestedGameId}");
            // Read the response content as a string
            var jsonResponseString = await response.Content.ReadAsStringAsync();
            // Parse the JSON response into a JObject
            var gottenGames = JsonConvert.DeserializeObject<List<GameDto>>(jsonResponseString);

            // Asserts
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Single(gottenGames);
            if(gottenGames is not null)
                foreach(GameDto game in gottenGames)
                    Assert.Equal(requestedGameId, game.Id);

        }

        // TODO: Expand this a bit? Maybe add a 404
        [Fact]
        public async Task GetGamesByDescription_ReturnsGameIfExists()
        {
            string gameDescription = "cartas"; // Change to a valid game ID for your test
            var response = await Client.GetAsync($"/games?descripiton={gameDescription}");
            // Read the response content as a string
            var jsonResponseString = await response.Content.ReadAsStringAsync();
            // Parse the JSON response into a JObject
            var gottenGames = JsonConvert.DeserializeObject<List<GameDto>>(jsonResponseString);

            // Asserts
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Single(gottenGames);
            if(gottenGames is not null)
                foreach(GameDto game in gottenGames)
                    Assert.Contains(gameDescription, game.GameDescription);
        }

        // TODO: Analisar melhor isto.
        [Fact]
        public async Task GetGames_ResponseHeadersContainExpectedValues()
        {
            // Act
            var response = await Client.GetAsync("/games");

            // Assert
            //Assert.True(response.Headers.Contains("Content-Type"));
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}