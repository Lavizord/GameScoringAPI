using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class PostGameEndpoints
{
    public static void MapPostGameEndpoints(this WebApplication app)
    {
        
        app.MapPost("/game", async (GameDto gameDto, GameDBContext context) =>
        {
            var game = new Game
            {
                GameName = gameDto.GameName,
                GameDescription = gameDto.GameDescription,
                MinPlayers = gameDto.MinPlayers,
                MaxPlayers = gameDto.MaxPlayers,
                AverageDuration = gameDto.AverageDuration
            };

            // We verify that the game we are creating meets the required stantarts.
            IResult gameValidationResult = GameValidation.IsGameValid(game);
            if(gameValidationResult is not Accepted)
                return gameValidationResult;

            // If everything is OK we add the game to our database.
            context.Games.Add(game);
            await context.SaveChangesAsync();

            return Results.Created($"/games/{game.Id}", game);
        })
        .WithName("PostGame")
        .WithTags("1. Games", "POST Endpoints")
        .WithOpenApi()
        .WithDescription("Creates a new game in the database using the provided game data. Returns 201 Created with the URL of the newly created game resource in the 'Location' header and the created game in the response body.")
        .Produces(StatusCodes.Status201Created, typeof(Game), "application/json");


        app.MapPost("/games", async (List<GameDto> gameDtos, GameDBContext context) =>
        {
            var createdGames = new List<Game>();

            foreach (var gameDto in gameDtos)
            {
                var game = new Game
                {
                    GameName = gameDto.GameName,
                    GameDescription = gameDto.GameDescription,
                    MinPlayers = gameDto.MinPlayers,
                    MaxPlayers = gameDto.MaxPlayers,
                    AverageDuration = gameDto.AverageDuration
                };
                 // We verify that the game we are creating meets the required stantarts.
                IResult gameValidationResult = GameValidation.IsGameValid(game);
                if(gameValidationResult is not Accepted)
                    return gameValidationResult;
                    
                context.Games.Add(game);
                createdGames.Add(game);
            }

            await context.SaveChangesAsync();

            return Results.Created("/games", createdGames);
        })
        .WithName("PostMultipleGames")
        .WithTags("1. Games", "POST Endpoints")
        .WithOpenApi()
        .WithDescription("Creates multiple games in the database using the provided game data. Returns 201 Created with the URL of the newly created games resource in the 'Location' header and the list of created games in the response body.")
        .Produces(StatusCodes.Status201Created, typeof(List<Game>), "application/json");

    }
}