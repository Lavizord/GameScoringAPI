using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public static class PostMatchEndpoints
{
    public static void MapPostMatchEndpoints(this WebApplication app)
    {    
        app.MapPost("/matches", async (MatchDto matchDto, GameDBContext context) =>
        {
            var gameExists = await context.Games.AnyAsync(g => g.Id == matchDto.GameId);
            if (!gameExists)
            {
                return Results.NotFound($"Game with ID {matchDto.GameId} not found.");
            }

            var match = new Match
            {
                GameId = matchDto.GameId,
                MatchDate = matchDto.MatchDate,
                Notes = matchDto.Notes
            };

            context.Matches.Add(match);
            await context.SaveChangesAsync();

            return Results.Created($"/matches/{match.Id}", match);
        })
        .WithName("PostMatch")
        .WithTags("2. Matches", "POST Endpoints")
        .WithOpenApi();    
    }
}