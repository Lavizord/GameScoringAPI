using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;


public class PostMatchDataPointDto
{
    public string PlayerName { get; set; }
    public int GamePoints { get; set; }
    public string PointsDescription { get; set; }
    public DateTime CreatedDate { get; set; }
}
public class ReturnPostMatchDataPointDto
{
    public int id { get; set; }
    public int MatchId { get; set; }
    public string PlayerName { get; set; }
    public int GamePoints { get; set; }
    public string PointsDescription { get; set; }
    public DateTime CreatedDate { get; set; }
}



public static class PostMatchDataPointEndpoints
{
    public static void MapPostMatchDataPointEndpoints(this WebApplication app)
    {   

        app.MapPost("/match-data-point/{MatchId}", async (int matchId, PostMatchDataPointDto dataPointDto, GameDBContext context) =>
        {
            // Check if the match exists and retrieve the match including the game
            var match = await context.Matches
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
            {
                return Results.NotFound($"Match with ID {matchId} not found. Can't POST the match-data-point.");
            }

            // Create the MatchDataPoint entity
            var matchDataPoint = new MatchDataPoint
            {
                MatchId = matchId,  // Set the foreign key reference
                PlayerName = dataPointDto.PlayerName,
                GamePoints = dataPointDto.GamePoints,
                PointsDescription = dataPointDto.PointsDescription,
                CreatedDate = DateTime.Now  // Assuming you want to set the created date here
            };

            // Add the new MatchDataPoint to the context and save changes
            context.MatchDataPoints.Add(matchDataPoint);
            await context.SaveChangesAsync();

            // Map the created MatchDataPoint to PostMatchDataPointDto
            var createdDataPointDto = new ReturnPostMatchDataPointDto
            {
                id = matchDataPoint.Id,
                MatchId = matchDataPoint.MatchId,
                PlayerName = matchDataPoint.PlayerName,
                GamePoints = matchDataPoint.GamePoints,
                PointsDescription = matchDataPoint.PointsDescription,
                CreatedDate = matchDataPoint.CreatedDate,
            };

            // Return the created data point DTO
            return Results.Created($"/match-data-points/{matchDataPoint.Id}", createdDataPointDto);
        })
        .WithName("PostMatchDataPoint")
        .WithTags("3. MatchDataPoints", "POST Endpoints")
        .WithOpenApi();


        app.MapPost("/match-data-points-all", async (List<MatchDataPointDto> matchDataPointDtos, GameDBContext context) =>
        {
            foreach (var dataPointDto in matchDataPointDtos)
            {
                // Check if the Game exists
                var game = await context.Games.FirstOrDefaultAsync(g => g.Id == dataPointDto.GameId);
                if (game == null)
                {
                    // If the game doesn't exist, create it
                    game = new Game
                    {
                        GameName = dataPointDto.GameName,
                        // Add other properties if necessary
                    };
                    context.Games.Add(game);
                    await context.SaveChangesAsync();
                }

                // Check if the Match exists
                var match = await context.Matches.FirstOrDefaultAsync(m => m.Id == dataPointDto.MatchId);
                if (match == null)
                {
                    // If the match doesn't exist, create it
                    match = new Match
                    {
                        GameId = game.Id,
                        MatchDate = DateTime.Now, // or any other date
                        // Add other properties if necessary
                    };
                    context.Matches.Add(match);
                    await context.SaveChangesAsync();
                }

                // Create the MatchDataPoint
                var matchDataPoint = new MatchDataPoint
                {
                    MatchId = match.Id,
                    PlayerName = dataPointDto.PlayerName,
                    GamePoints = dataPointDto.GamePoints,
                    PointsDescription = dataPointDto.PointsDescription,
                    CreatedDate = dataPointDto.CreatedDate
                };
                context.MatchDataPoints.Add(matchDataPoint);
            }

            await context.SaveChangesAsync();

            return Results.Created("/match-data-points-all", matchDataPointDtos);
        })
        .WithName("PostMatchDataPointsAll")
        .WithTags("3. MatchDataPoints", "POST Endpoints", "9. FrontEnd - Mockup")
        .WithOpenApi();  
    }
}