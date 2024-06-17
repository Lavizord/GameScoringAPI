public static class DeleteMatchEndpoints
{
    public static void MapDeleteMatchEndpoints(this WebApplication app)
    {        
        app.MapDelete("/match-and-data-points/{id}", async (int id, GameDBContext context) =>
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
        .WithTags("2. Matches", "DELETE Endpoints", "9. FrontEnd - Mockup")
        .WithOpenApi()
        .WithDescription
        (
            "Deletes a match and all associated data points from the database identified by the provided ID. Returns 404 Not Found if the match with the specified ID is not found. Upon successful deletion, returns 204 No Content."
        )
        .Produces(StatusCodes.Status404NotFound, typeof(string), "application/json")
        .Produces(StatusCodes.Status204NoContent, typeof(void), "application/json");
    }
}