using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public static class DeleteMatchDataPointEndpoints
{
    public static void MapDeleteMatchDataPointEndpoints(this WebApplication app)
    {
        app.MapDelete("/match-data-point/{id}", async (int id, GameDBContext context) =>
        {
            var matchdp = await context.MatchDataPoints.FindAsync(id);
            if (matchdp == null)
                return Results.NotFound($"Match Data Point with ID {id} not found.");

            // Delete the match
            context.MatchDataPoints.Remove(matchdp);
            await context.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteMatchDataPoint")
        .WithTags("3. MatchDataPoints", "DELETE Endpoints")
        .WithOpenApi();
    }
}