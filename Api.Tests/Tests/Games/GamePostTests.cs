using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;

namespace Api.Tests
{
    public class GamePostTests :TestBase
    {
        public GamePostTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateSingleGame_ReturnsCreated()
        {            
            var result = await Client.PostAsJsonAsync("/game", new GameDto
            {
                GameName = "UnitTestName",
                GameDescription = "This is a test game description."
            });
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task CreateSingleGame_BadRequest_InvalidData()
        {
            // Test for invalid game Name.
            var result = await Client.PostAsJsonAsync("/game", new GameDto
            {
                GameName = ""
            });
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            // Test for invalid MinPlayers.
            result = await Client.PostAsJsonAsync("/game", new GameDto
            {
                GameName = "TESTE", MinPlayers = -1
            });
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            
            // Test for invalid MaxPlayers.
            result = await Client.PostAsJsonAsync("/game", new GameDto
            {
                GameName = "TESTE2", MinPlayers = -1
            });
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            // Test for invalid AverageDuration.
            result = await Client.PostAsJsonAsync("/game", new GameDto
            {
                GameName = "TESTE2", AverageDuration = -1
            });
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateSingleGame_ReturnsCorrectData()
        {
            // Create the game DTO to send in the POST request
            var gameToCreate = new GameDto
            {
                GameName = "GameTest",
                GameDescription = "This is a test game description.",
                MinPlayers = 2,
                MaxPlayers = 6,
                AverageDuration = 90,
                MatchesCount = 0
            };

            // Send the POST request
            var result = await Client.PostAsJsonAsync("/game", gameToCreate);

            // Read and deserialize the response content
            var jsonResponseString = await result.Content.ReadAsStringAsync();
            var createdGame = Newtonsoft.Json.JsonConvert.DeserializeObject<GameDto>(jsonResponseString);

            // Create the expected game DTO with the expected Id
            var expectedGame = new GameDto
            {
                Id = createdGame.Id,  // Assume the Id is set by the server and is not known beforehand
                GameName = "GameTest",
                GameDescription = "This is a test game description.",
                MinPlayers = 2,
                MaxPlayers = 6,
                AverageDuration = 90,
                MatchesCount = 0
            };

            // Assert that the created game matches the expected game
            Assert.Equal(expectedGame.Id, createdGame.Id);
            Assert.Equal(expectedGame.GameName, createdGame.GameName);
            Assert.Equal(expectedGame.GameDescription, createdGame.GameDescription);
            Assert.Equal(expectedGame.MinPlayers, createdGame.MinPlayers);
            Assert.Equal(expectedGame.MaxPlayers, createdGame.MaxPlayers);
            Assert.Equal(expectedGame.AverageDuration, createdGame.AverageDuration);
            Assert.Equal(expectedGame.MatchesCount, createdGame.MatchesCount);
        }

        [Fact]
        public async Task CreateMultipleGames_ReturnsCreated()
        {
            var result = await Client.PostAsJsonAsync("/games", new List<GameDto>
            {
                new GameDto { GameName = "Game 1" }, new GameDto { GameName = "Game 2" }, new GameDto { GameName = "Game 3" }
            });
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }

        [Fact]
        public async Task CreateMultipleGames_BadRequest_InvalidData()
        {           
            var result = await Client.PostAsJsonAsync("/games", new List<GameDto>
            {
                new GameDto { GameName = "Game 1" }, new GameDto { GameName = "" }, new GameDto { GameName = "Game 3" }
            });
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            
            result = await Client.PostAsJsonAsync("/games", new List<GameDto>
            {
                new GameDto { GameName = "Game 1", MinPlayers = 0}, new GameDto { GameName = "Game 2",  MinPlayers = -1 }, new GameDto { GameName = "Game 3", MinPlayers = 5 }
            });
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            result = await Client.PostAsJsonAsync("/games", new List<GameDto>
            {
                new GameDto { GameName = "Game 1"}, new GameDto { GameName = "Game 2",  AverageDuration = -1 }, new GameDto { GameName = "Game 3"}
            });
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task CreateMultipleGames_ReturnsCorrectData()
        {
            // Create the list of game DTOs to send in the POST request
            var gamesToCreate = new List<GameDto>
            {
                new GameDto
                {
                    GameName = "GameTest1",
                    GameDescription = "This is a test game description 1.",
                    MinPlayers = 2,
                    MaxPlayers = 6,
                    AverageDuration = 90,
                    MatchesCount = 0
                },
                new GameDto
                {
                    GameName = "GameTest2",
                    GameDescription = "This is a test game description 2.",
                    MinPlayers = 3,
                    MaxPlayers = 5,
                    AverageDuration = 60,
                    MatchesCount = 0
                },
                new GameDto
                {
                    GameName = "GameTest3",
                    GameDescription = "This is a test game description 3.",
                    MinPlayers = 1,
                    MaxPlayers = 4,
                    AverageDuration = 30,
                    MatchesCount = 0
                }
            };

            // Send the POST request
            var result = await Client.PostAsJsonAsync("/games", gamesToCreate);

            // Log the status code
            Console.WriteLine($"Status Code: {result.StatusCode}");

            // Read and log the raw response content
            var jsonResponseString = await result.Content.ReadAsStringAsync();
            Console.WriteLine("Response JSON: " + jsonResponseString);

            // Deserialize the response content
            var createdGames = JsonConvert.DeserializeObject<List<GameDto>>(jsonResponseString);

            // Assert that the number of created games matches the number of games sent
            Assert.Equal(gamesToCreate.Count, createdGames.Count);

            for (int i = 0; i < gamesToCreate.Count; i++)
            {
                var expectedGame = gamesToCreate[i];
                var createdGame = createdGames[i];

                // Update the expected game with the returned Id
                expectedGame.Id = createdGame.Id;

                // Assert that each created game matches the expected game
                Assert.Equal(expectedGame.Id, createdGame.Id);
                Assert.Equal(expectedGame.GameName, createdGame.GameName);
                Assert.Equal(expectedGame.GameDescription, createdGame.GameDescription);
                Assert.Equal(expectedGame.MinPlayers, createdGame.MinPlayers);
                Assert.Equal(expectedGame.MaxPlayers, createdGame.MaxPlayers);
                Assert.Equal(expectedGame.AverageDuration, createdGame.AverageDuration);
                Assert.Equal(expectedGame.MatchesCount, createdGame.MatchesCount);
            }
        
        }
    
        [Fact]
        public async Task CreateSingleGame_ResponseHeadersContainExpectedValues()
        {
            // Act
            var response = await Client.PostAsJsonAsync("/game", new GameDto
            {
                GameName = "UnitTestName"
            });
            // Assert
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
        [Fact]
        public async Task CreateMultipleGames_ResponseHeadersContainExpectedValues()
        {
            // Create the list of game DTOs to send in the POST request
            var gamesToCreate = new List<GameDto>
            {
                new GameDto
                {
                    GameName = "GameTest1",
                    GameDescription = "This is a test game description 1.",
                    MinPlayers = 2,
                    MaxPlayers = 6,
                    AverageDuration = 90,
                    MatchesCount = 0
                }
            };

            // Send the POST request
            var response = await Client.PostAsJsonAsync("/games", gamesToCreate);
            // Assert
            Assert.NotNull(response.Content.Headers.ContentType);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}