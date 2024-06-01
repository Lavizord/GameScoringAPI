using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public static class PutGameEndpoints
{
    public static void MapPutGameEndpoints(this WebApplication app)
    {
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
        .WithTags("1. Games", "PUT Endpoints")
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
        .WithTags("1. Games", "PUT Endpoints")
        .WithOpenApi();
        
    }
}