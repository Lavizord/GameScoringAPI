using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public static class DeleteGameEndpoints
{
    public static void MapDeleteGameEndpoints(this WebApplication app)
    {

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
        .WithTags("1. Games", "DELETE Endpoints", "9. FrontEnd - Mockup")
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
                    return Results.NotFound($"Game with ID {id} not found. No games were deleted."); // Return a 404 Not Found response
                }

                context.Games.Remove(game);
            }

            await context.SaveChangesAsync();

            return Results.NoContent(); // Return success response
        })
        .WithName("DeleteMultipleGames")
        .WithTags("1. Games", "DELETE Endpoints")
        .WithOpenApi();  
        
    }
}