using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public static class GetMatchDataPointEndpoints
{
    public static void MapGetMatchDataPointEndpoints(this WebApplication app)
    {        
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
                    isMatchFinished = dp.Match.isFinished,
                    GameId = dp.Match.GameId,  // Access GameId through Match
                    GameName = dp.Match.Game.GameName  // Access GameName through Match
                })
                .ToListAsync();

            return Results.Ok(matchDataPoints);
        })
        .WithName("GetMatchDataPoints")
        .WithTags("3. MatchDataPoints", "GET Endpoints", "9. FrontEnd - Mockup")
        .WithOpenApi();

    }
}