using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public static class DeleteMatchEndpoints
{
    public static void MapDeleteMatchEndpoints(this WebApplication app)
    {        
        app.MapDelete("/match-and-data-pointss/{id}", async (int id, GameDBContext context) =>
        {
            var match = await context.Matches.FindAsync(id);
            if (match == null)
            {
                return Results.NotFound($"Match with ID {id} not found.");
            }

            // Retrieve and delete all MatchDataPoint records with the specified MatchID
            var dataPoints = context.MatchDataPoints.Where(dp => dp.MatchId == id);
            context.MatchDataPoints.RemoveRange(dataPoints);

            // Delete the match
            context.Matches.Remove(match);
            await context.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteMatchAndAllDataPoints")
        .WithTags("2. Matches", "DELETE Endpoints")
        .WithOpenApi();
    }
}