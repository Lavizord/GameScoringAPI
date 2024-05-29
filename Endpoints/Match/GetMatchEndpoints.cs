using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public class MatchForMatchDto
{
    public int MatchId { get; set; }
    public int GameId { get; set; }
    public DateTime MatchDate { get; set; }
    public string? Notes { get; set; }
    public bool isFinished { get; set; }
    public List<MatchDataPointForMatchDto> MatchDataPoints { get; set; } = new List<MatchDataPointForMatchDto>();
}

public class MatchDataPointForMatchDto
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public int GamePoints { get; set; }
    public string PointsDescription { get; set; }
    public DateTime CreatedDate { get; set; }
}


public static class GetMatchEndpoints
{
    public static void MapGetMatchEndpoints(this WebApplication app)
    {    
        app.MapGet("/match/{id}", async (int id, bool? includeDataPoints, GameDBContext context) =>
        {
            var query = context.Matches.AsQueryable();
            query = query.Where(m => m.Id == id);

            var match = await query
                .Select(m => new MatchForMatchDto
                {
                    MatchId = m.Id,
                    GameId = m.GameId,
                    MatchDate = m.MatchDate,
                    Notes = m.Notes,
                    isFinished = m.isFinished,
                    MatchDataPoints = includeDataPoints == true
                        ? m.MatchDataPoints.Select(dp => new MatchDataPointForMatchDto
                        {
                            Id = dp.Id,
                            PlayerName = dp.PlayerName,
                            GamePoints = dp.GamePoints,
                            PointsDescription = dp.PointsDescription,
                            CreatedDate = dp.CreatedDate
                        }).ToList()
                        : new List<MatchDataPointForMatchDto>()
                })
                .FirstOrDefaultAsync();
            if (match == null)
                return Results.NotFound($"Match with ID {id} not found.");

            return Results.Ok(match);
        })
        .WithName("GetMatches")
        .WithTags("2. Matches", "GET Endpoints")
        .WithOpenApi(); 


        app.MapGet("/matches", async (bool? includeDataPoints, int? gameId, GameDBContext context) =>
        {
            var query = context.Matches.AsQueryable();

            // Apply filter if gameId is provided
            if (gameId.HasValue)
                query = query.Where(m => m.GameId == gameId.Value);
            

            var matches = await query
                .Select(m => new MatchForMatchDto
                {
                    MatchId = m.Id,
                    GameId = m.GameId,
                    MatchDate = m.MatchDate,
                    Notes = m.Notes,
                    isFinished = m.isFinished,
                    MatchDataPoints = includeDataPoints == true
                        ? m.MatchDataPoints.Select(dp => new MatchDataPointForMatchDto
                        {
                            Id = dp.Id,
                            PlayerName = dp.PlayerName,
                            GamePoints = dp.GamePoints,
                            PointsDescription = dp.PointsDescription,
                            CreatedDate = dp.CreatedDate
                        }).ToList()
                        : new List<MatchDataPointForMatchDto>()
                })
                .ToListAsync();

            if (matches == null || matches.Count == 0)
                return Results.NotFound($"No matches found.");

            return Results.Ok(matches);
        })
        .WithName("GetAllMatches")
        .WithTags("2. Matches", "GET Endpoints")
        .WithOpenApi();
           
    }
}