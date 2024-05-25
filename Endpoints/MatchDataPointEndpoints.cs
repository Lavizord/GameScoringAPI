using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class MatchDataPointEndpoints
{
    public static void MapMatchDataPointEndpoints(this WebApplication app)
    {
        app.MapPost("/match-data-point", async (MatchDataPointDto dataPointDto, GameDBContext context) =>
        {
            var gameExists = await context.Games.AnyAsync(g => g.Id == dataPointDto.GameId);
            if (!gameExists)
            {
                return Results.NotFound($"Game with ID {dataPointDto.GameId} not found.");
            }

            var matchExists = await context.Matches.AnyAsync(m => m.Id == dataPointDto.MatchId);
            if (!matchExists)
            {
                return Results.NotFound($"Match with ID {dataPointDto.MatchId} not found.");
            }

            var matchDataPoint = new MatchDataPoint
            {
                GameId = dataPointDto.GameId,
                MatchId = dataPointDto.MatchId,
                PlayerName = dataPointDto.PlayerName,
                GamePoints = dataPointDto.GamePoints,
                PointsDescription = dataPointDto.PointsDescription
            };

            context.MatchDataPoints.Add(matchDataPoint);
            await context.SaveChangesAsync();

            return Results.Created($"/match-data-points/{matchDataPoint.Id}", matchDataPoint);
        })
        .WithName("PostMatchDataPoint")
        .WithTags("Match Data Points")
        .WithOpenApi();

        app.MapPost("/match-multiple-data-points", async (List<MatchDataPointDto> dataPointDtos, GameDBContext context) =>
        {
            var gameIds = dataPointDtos.Select(dp => dp.GameId).Distinct();
            var matchIds = dataPointDtos.Select(dp => dp.MatchId).Distinct();

            var gamesExist = await context.Games.Where(g => gameIds.Contains(g.Id)).Select(g => g.Id).ToListAsync();
            if (gamesExist.Count != gameIds.Count())
            {
                return Results.NotFound("One or more games not found.");
            }

            var matchesExist = await context.Matches.Where(m => matchIds.Contains(m.Id)).Select(m => m.Id).ToListAsync();
            if (matchesExist.Count != matchIds.Count())
            {
                return Results.NotFound("One or more matches not found.");
            }

            var matchDataPoints = dataPointDtos.Select(dataPointDto => new MatchDataPoint
            {
                GameId = dataPointDto.GameId,
                MatchId = dataPointDto.MatchId,
                PlayerName = dataPointDto.PlayerName,
                GamePoints = dataPointDto.GamePoints,
                PointsDescription = dataPointDto.PointsDescription
            }).ToList();

            context.MatchDataPoints.AddRange(matchDataPoints);
            await context.SaveChangesAsync();

            return Results.Created("/match-data-points", matchDataPoints);
        })
        .WithName("PostMultipleMatchDataPoints")
        .WithTags("Match Data Points")
        .WithOpenApi();
    }
}