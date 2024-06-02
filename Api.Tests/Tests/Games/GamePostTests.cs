using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Api.Tests
{
    public class GamePostTests
    {

        [Fact]
        public async Task CreateSingleGame()
        {
            await using var application = new WebApplicationFactory<Program>();

            var client = application.CreateClient();
            
            var result = await client.PostAsJsonAsync("/game", new GameDto
            {
                GameName = "UnitTestName",
                GameDescription = "This is a test game description."
            });
        
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            //Assert.Equal("\"Maarten Balliauw created.\"", await result.Content.ReadAsStringAsync());
        }
    }
}