using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

            context.Games.Add(game);
            await context.SaveChangesAsync();

            return Results.Created($"/games/{game.Id}", game);
        })
        .WithName("PostGame")
        .WithTags("1. Games", "POST Endpoints")
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
        .WithTags("1. Games", "POST Endpoints", "9. FrontEnd - Mockup")
        .WithOpenApi();
    }
}