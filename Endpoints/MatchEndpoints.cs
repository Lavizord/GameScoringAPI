using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class MatchEndpoints
{
    public static void MapMatchEndpoints(this IEndpointRouteBuilder app)
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
        .WithOpenApi();
    }
}