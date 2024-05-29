using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;


public class MatchForGameDto
{
    public int MatchId { get; set; }
    public DateTime MatchDate { get; set; }
    public string? Notes { get; set; }
    public bool isFinished { get; set; }
    public List<MatchDataPointForMatchDto> MatchDataPoints { get; set; } = new List<MatchDataPointForMatchDto>();
}

public static class GetGameEndpoints
{
    public static void MapGetGameEndpoints(this WebApplication app)
    {

        app.MapGet("/game/{id}", async (int id, GameDBContext context) =>
        {
            var game = await context.Games.FindAsync(id);
            if (game == null)
            {
                return Results.NotFound($"Game with ID {id} not found.");
            }

            return Results.Ok(game);
        })
        .WithName("GetGameById")
        .WithTags("1. Games", "GET Endpoints", "9. FrontEnd - Mockup")
        .WithOpenApi();


        app.MapGet("/games", async (int? id, string? descripiton, GameDBContext context) =>
        {
            IQueryable<Game> gamesQuery = context.Games;
            if(id != null)
                gamesQuery = gamesQuery.Where(g => g.Id == id);

            if (!string.IsNullOrEmpty(descripiton))
                gamesQuery = gamesQuery.Where(g => g.GameDescription.Contains(descripiton));
            
            var games = await context.Games.ToListAsync();

            return Results.Ok(games);
        })
        .WithName("GetAllGames")
        .WithTags("1. Games", "GET Endpoints", "9. FrontEnd - Mockup")
        .WithOpenApi();


        app.MapGet("/games-with-matches-and-data-points", async (string? gameName, DateTime? matchDateAfter, DateTime? matchDateBefore, GameDBContext context) =>
        {
            IQueryable<Game> gamesQuery = context.Games.Include(g => g.Matches).ThenInclude(m => m.MatchDataPoints);

            if (!string.IsNullOrEmpty(gameName))
                gamesQuery = gamesQuery.Where(g => g.GameName.Contains(gameName));

            if (matchDateAfter.HasValue)
                gamesQuery = gamesQuery.Where(g => g.Matches.Any(m => m.MatchDate >= matchDateAfter));
            
            if (matchDateBefore.HasValue)
                gamesQuery = gamesQuery.Where(g => g.Matches.Any(m => m.MatchDate <= matchDateBefore));

            var games = await gamesQuery.ToListAsync();

            var gamesWithMatchesDto = games.Select(game => new GameWithMatchDataPointDto
            {
                Id = game.Id,
                GameName = game.GameName,
                GameDescription = game.GameDescription,
                MinPlayers = game.MinPlayers,
                MaxPlayers = game.MaxPlayers,
                AverageDuration = game.AverageDuration,
                Matches = game.Matches.Select(match => new MatchForGameDto
                {
                    MatchId = match.Id,
                    MatchDate = match.MatchDate,
                    Notes = match.Notes,
                    MatchDataPoints = match.MatchDataPoints.Select(dp => new MatchDataPointForMatchDto
                    {
                            Id = dp.Id,
                            PlayerName = dp.PlayerName,
                            GamePoints = dp.GamePoints,
                            PointsDescription = dp.PointsDescription,
                            CreatedDate = dp.CreatedDate
                    }).ToList()
                }).ToList()
            }).ToList();

            return Results.Ok(gamesWithMatchesDto);
        })
        .WithName("GetGamesWithMatchesAndDataPoints")
        .WithTags("0. Full Dataset", "GET Endpoints")
        .WithOpenApi();
    }
}