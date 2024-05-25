using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public static class GameEndpoints
{
    public static void MapGameEndpoints(this WebApplication app)
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

            context.Games.Add(game);
            await context.SaveChangesAsync();

            return Results.Created($"/games/{game.Id}", game);
        })
        .WithName("PostGame")
        .WithTags("Games", "POST Endpoints")
        .WithOpenApi();

        app.MapPut("/game/{id}", async (int id, GameDto gameDto, GameDBContext context) =>
        {
            var existingGame = await context.Games.FindAsync(id);
            if (existingGame == null)
            {
                return Results.NotFound($"Game with ID {id} not found.");
            }

            existingGame.GameName = gameDto.GameName;
            existingGame.GameDescription = gameDto.GameDescription;
            existingGame.MinPlayers = gameDto.MinPlayers;
            existingGame.MaxPlayers = gameDto.MaxPlayers;
            existingGame.AverageDuration = gameDto.AverageDuration;

            await context.SaveChangesAsync();

            return Results.Ok(existingGame);
        })
        .WithName("PutGame")
        .WithTags("Games", "PUT Endpoints")
        .WithOpenApi();

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

                context.Games.Add(game);
                createdGames.Add(game);
            }

            await context.SaveChangesAsync();

            return Results.Created("/games", createdGames);
        })
        .WithName("PostMultipleGames")
        .WithTags("Games", "POST Endpoints")
        .WithOpenApi();

        app.MapPut("/games", async (List<GameDto> gameDtos, GameDBContext context) =>
        {
            var updatedGames = new List<Game>();

            foreach (var gameDto in gameDtos)
            {
                var existingGame = await context.Games.FindAsync(gameDto.Id);
                if (existingGame != null)
                {
                    existingGame.GameName = gameDto.GameName;
                    existingGame.GameDescription = gameDto.GameDescription;
                    existingGame.MinPlayers = gameDto.MinPlayers;
                    existingGame.MaxPlayers = gameDto.MaxPlayers;
                    existingGame.AverageDuration = gameDto.AverageDuration;

                    updatedGames.Add(existingGame);
                }
            }

            await context.SaveChangesAsync();

            return Results.Ok(updatedGames);
        })
        .WithName("PutMultipleGames")
        .WithTags("Games", "PUT Endpoints")
        .WithOpenApi();

        app.MapDelete("/game/{id}", async (int id, GameDBContext context) =>
        {
            var game = await context.Games.FindAsync(id);
            if (game == null)
            {
                return Results.NotFound($"Game with ID {id} not found.");
            }

            context.Games.Remove(game);
            await context.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteGame")
        .WithTags("Games", "DELETE Endpoints")
        .WithOpenApi();

        app.MapDelete("/games", async (GameDBContext context, params int[] gameIds) =>
        {
            // Check if gameIds is null or empty
            if (gameIds == null || gameIds.Length == 0)
            {
                return Results.BadRequest("No game IDs provided."); // Return a 400 Bad Request response
            }

            // Iterate through each gameId in the list and delete the corresponding game
            foreach (var id in gameIds)
            {
                var game = await context.Games.FindAsync(id);
                if (game == null)
                {
                    return Results.NotFound($"Game with ID {id} not found."); // Return a 404 Not Found response
                }

                context.Games.Remove(game);
            }

            await context.SaveChangesAsync();

            return Results.NoContent(); // Return success response
        })
        .WithName("DeleteMultipleGames")
        .WithTags("Games", "DELETE Endpoints")
        .WithOpenApi();

    }
}