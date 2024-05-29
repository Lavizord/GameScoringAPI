using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

public class PutSingleMatchDto
{
    public int GameId { get; set; }
    public DateTime MatchDate { get; set; }
    public string Notes { get; set; }
    public bool isFinished { get; set; }
}


public static class PutMatchEndpoints
{
    public static void MapPutMatchEndpoints(this WebApplication app)
    {      
        app.MapPut("/match/{id}", async (int id, PutSingleMatchDto matchDto, GameDBContext context) =>
        {
            var matchExists = await context.Matches.AnyAsync(m => m.Id == id);
            if (!matchExists)
                return Results.NotFound($"Match with ID {id} not found. Can't Update.");

            var gameExists = await context.Games.AnyAsync(g => g.Id == matchDto.GameId);
            if (!gameExists)
                return Results.NotFound($"Game with ID {matchDto.GameId} not found. Cant Update.");
            
            // TODO: I believe these checks should be handled elsewere, this will ensure we can reuse them.
            // Here we determinte a few things for a later check.
            // We determine weather or not a match already has any DataPoint.
            var matchHasDataPoints = await context.MatchDataPoints.AnyAsync(mdt => mdt.MatchId == id);
            // We also determine if our GameID was changed.
            Match? dbSavedMatchRecord = await context.Matches.FirstOrDefaultAsync(m => m.Id == id);
            var gameIdChanged = false;
            if(dbSavedMatchRecord is not null)
                gameIdChanged = dbSavedMatchRecord.GameId != matchDto.GameId;
            
            // Now, if we have dataPoints associated with the match and the match game ID is being changed, we need to return an error.
            // This is to garantee that our points and its descriptions dont end up associated with another game...
            if (matchHasDataPoints && gameIdChanged ) 
                return Results.Conflict($"It seems the GameID of the saved match is being changed (from [{dbSavedMatchRecord.GameId}] to [{matchDto.GameId}]). The opearation will be aborted since the Match atlerady has data points.");
            

            // If we pass all those checks, we can then update our Match.
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
        .WithName("PutMatch")
        .WithTags("2. Matches", "PUT Endpoints")
        .WithOpenApi();    
    }

    // TODO: Delete match, with all associated datapoints.
}