using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

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
        .WithTags("Games", "GET Endpoints", "FrontEnd - Mockup")
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
        .WithTags("Games", "GET Endpoints", "FrontEnd - Mockup")
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
                Matches = game.Matches.Select(match => new MatchDto
                {
                    MatchId = match.Id,
                    GameId = match.GameId,
                    MatchDate = match.MatchDate,
                    Notes = match.Notes,
                    MatchDataPoints = match.MatchDataPoints.Select(dp => new MatchDataPointDto
                    {
                        MatchId = dp.MatchId,
                        PlayerName = dp.PlayerName,
                        GamePoints = dp.GamePoints,
                        PointsDescription = dp.PointsDescription
                    }).ToList()
                }).ToList()
            }).ToList();

            return Results.Ok(gamesWithMatchesDto);
        })
        .WithName("GetGamesWithMatchesAndDataPoints")
        .WithTags("Games Matches DataPoints", "GET Endpoints", "FrontEnd - Mockup")
        .WithOpenApi();
    }
}