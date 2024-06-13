using GameScoringAPI.Mapper;
using Microsoft.EntityFrameworkCore;

public class MatchForMatchDto
{
    public int MatchId { get; set; }
    public int GameId { get; set; }
    public DateTime MatchDate { get; set; }
    public string? Notes { get; set; }
    public int PlayerCount { get; set; }
    public int NumberOfPlayers { get; set; }
    public bool isFinished { get; set; }
    public List<MatchDataPointForMatchDto>? MatchDataPoints { get; set; } = new List<MatchDataPointForMatchDto>();
    public MatchStatsDto? MatchStats { get; set; } = new MatchStatsDto();

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
            // Our Queriyable Mathch and corresponding DTO.
            IQueryable<MatchForMatchDto> matchesQuery;
            IQueryable<Match> query = context.Matches.AsQueryable();

            query = query.Where(m => m.Id == id);

            // Selects and mapps the data from the models to the DTOs.
            matchesQuery = MatchMapper.SelectAndMapToDTO(query, includeDataPoints);

            // Execute our query.
            MatchForMatchDto? match = await matchesQuery.FirstOrDefaultAsync();
           
            if (match == null)
                return Results.NotFound($"Match with ID {id} not found.");

            // Include datapoints if necessary.
            if (includeDataPoints is not null && (bool)includeDataPoints) 
            {
                match.MatchStats = MatchMapper.CreateMathStatsFor(match);
                // Now let's calculate the winning player for each match
                MatchMapper.CalculateWinnerFor(match);
            }

            return Results.Ok(match);
        })
        .WithName("GetMatch")
        .WithTags("2. Matches", "GET Endpoints")
        .WithOpenApi(); 


        app.MapGet("/matches", async (bool? includeDataPoints, int? gameId, GameDBContext context) =>
        {
            // Our Queriyable Mathch and corresponding DTO.
            IQueryable<MatchForMatchDto> matchesQuery;
            IQueryable<Match> query = context.Matches.AsQueryable();
            
            // Apply filter if gameId is provided.
            if (gameId.HasValue) 
                query = query.Where(m => m.GameId == gameId.Value);

            // Selects and mapps the data from the models to the DTOs.
            matchesQuery = MatchMapper.SelectAndMapToDTO(query, includeDataPoints);

            // Execute our query.
            List<MatchForMatchDto> matches = await matchesQuery.ToListAsync();

            // Include datapoints if necessary.
            if (includeDataPoints is not null && (bool)includeDataPoints) 
                foreach (MatchForMatchDto match in matches)
                {
                    match.MatchStats = MatchMapper.CreateMathStatsFor(match);
                    // Now let's calculate the winning player for each match
                    MatchMapper.CalculateWinnerFor(match);
                }
                
            // Final endpoint not found validation.            
            if(matches == null || matches.Count == 0)
                return Results.NotFound($"No matches found.");

            return Results.Ok(matches);
        })
        .WithName("GetAllMatches")
        .WithTags("2. Matches", "GET Endpoints")
        .WithOpenApi();
           
    }
}