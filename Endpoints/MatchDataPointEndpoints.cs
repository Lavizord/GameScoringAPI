using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class MatchDataPointEndpoints
{
    public static void MapMatchDataPointEndpoints(this WebApplication app)
    {
        app.MapPost("/match-data-point", async (MatchDataPointDto dataPointDto, GameDBContext context) =>
        {
            // Check if the match exists and retrieve the match including the game
            var match = await context.Matches
                .Include(m => m.Game)
                .FirstOrDefaultAsync(m => m.Id == dataPointDto.MatchId);

            if (match == null)
            {
                return Results.NotFound($"Match with ID {dataPointDto.MatchId} not found.");
            }

            // Create the MatchDataPoint entity
            var matchDataPoint = new MatchDataPoint
            {
                MatchId = dataPointDto.MatchId,
                PlayerName = dataPointDto.PlayerName,
                GamePoints = dataPointDto.GamePoints,
                PointsDescription = dataPointDto.PointsDescription,
                CreatedDate = DateTime.Now  // Assuming you want to set the created date here
            };

            // Add the new MatchDataPoint to the context and save changes
            context.MatchDataPoints.Add(matchDataPoint);
            await context.SaveChangesAsync();

            // Map the created MatchDataPoint to MatchDataPointDto
            var createdDataPointDto = new MatchDataPointDto
            {
                Id = matchDataPoint.Id,
                MatchId = matchDataPoint.MatchId,
                PlayerName = matchDataPoint.PlayerName,
                GamePoints = matchDataPoint.GamePoints,
                PointsDescription = matchDataPoint.PointsDescription,
                CreatedDate = matchDataPoint.CreatedDate,
                GameName = match.Game.GameName  // Assuming you include GameName in the DTO
            };

            // Return the created data point DTO
            return Results.Created($"/match-data-points/{matchDataPoint.Id}", createdDataPointDto);
        })
        .WithName("PostMatchDataPoint")
        .WithTags("MatchDataPoints", "POST Endpoints")
        .WithOpenApi();

        app.MapPost("/match-data-points-multiple", async (List<MatchDataPointDto> dataPointDtos, GameDBContext context) =>
        {
            var matchIds = dataPointDtos.Select(dp => dp.MatchId).Distinct();

            var matchesExist = await context.Matches
                .Where(m => matchIds.Contains(m.Id))
                .Select(m => m.Id)
                .ToListAsync();

            if (matchesExist.Count != matchIds.Count())
            {
                return Results.NotFound("One or more matches not found.");
            }

            var matchDataPoints = dataPointDtos.Select(dataPointDto => new MatchDataPoint
            {
                MatchId = dataPointDto.MatchId,
                PlayerName = dataPointDto.PlayerName,
                GamePoints = dataPointDto.GamePoints,
                PointsDescription = dataPointDto.PointsDescription,
                CreatedDate = DateTime.Now  // Assuming you want to set the created date here
            }).ToList();

            context.MatchDataPoints.AddRange(matchDataPoints);
            await context.SaveChangesAsync();

            // Optionally, map to DTOs before returning
            var createdDataPointsDtos = matchDataPoints.Select(dp => new MatchDataPointDto
            {
                Id = dp.Id,
                MatchId = dp.MatchId,
                PlayerName = dp.PlayerName,
                GamePoints = dp.GamePoints,
                PointsDescription = dp.PointsDescription,
                CreatedDate = dp.CreatedDate,
                GameName = dp.Match.Game.GameName  // Assuming you include GameName in the DTO
            }).ToList();

            return Results.Created("/match-data-points", createdDataPointsDtos);
        })
        .WithName("PostMultipleMatchDataPoints")
        .WithTags("MatchDataPoints", "POST Endpoints")
        .WithOpenApi();

        
        app.MapGet("/match-data-points-all", async (GameDBContext context) =>
        {
            var matchDataPoints = await context.MatchDataPoints
                .Include(dp => dp.Match)       // Include the related Match entity
                .ThenInclude(m => m.Game)      // Then include the related Game entity
                .Select(dp => new MatchDataPointDto
                {
                    Id = dp.Id,
                    MatchId = dp.MatchId,
                    PlayerName = dp.PlayerName,
                    GamePoints = dp.GamePoints,
                    PointsDescription = dp.PointsDescription,
                    CreatedDate = dp.CreatedDate,
                    GameID = dp.Match.GameId,  // Access GameId through Match
                    GameName = dp.Match.Game.GameName  // Access GameName through Match
                })
                .ToListAsync();

            return Results.Ok(matchDataPoints);
        })
        .WithName("GetMatchDataPoints")
        .WithTags("MatchDataPoints", "GET Endpoints", "FrontEnd - Mockup")
        .WithOpenApi();

        app.MapPost("/match-data-points-all", async (List<MatchDataPointDto> matchDataPointDtos, GameDBContext context) =>
        {
            foreach (var dataPointDto in matchDataPointDtos)
            {
                // Check if the Game exists
                var game = await context.Games.FirstOrDefaultAsync(g => g.Id == dataPointDto.GameID);
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
    .WithTags("MatchDataPoints", "POST Endpoints", "FrontEnd - Mockup")
    .WithOpenApi();
    }
}