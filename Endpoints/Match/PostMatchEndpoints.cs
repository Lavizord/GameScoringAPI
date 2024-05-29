using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

// Declared DTO here because I only intend on using it here.
public class PostSingleMatchDto
{
    public int GameId { get; set; }
    public DateTime MatchDate { get; set; }
    public string Notes { get; set; }
    public bool isFinished { get; set; }
}


public static class PostMatchEndpoints
{
    public static void MapPostMatchEndpoints(this WebApplication app)
    {    
        app.MapPost("/match", async (PostSingleMatchDto matchDto, GameDBContext context) =>
        {
            var gameExists = await context.Games.AnyAsync(g => g.Id == matchDto.GameId);
            if (!gameExists)
                return Results.NotFound($"Game with ID {matchDto.GameId} not found.");
            
            var match = new Match
            {
                GameId = matchDto.GameId,
                MatchDate = matchDto.MatchDate,
                Notes = matchDto.Notes,
                isFinished = matchDto.isFinished
            };
            context.Matches.Add(match);
            await context.SaveChangesAsync();
            return Results.Created($"/matches/{match.Id}", match);
        })
        .WithName("PostMatch")
        .WithTags("2. Matches", "POST Endpoints")
        .WithOpenApi();    
    }
}
