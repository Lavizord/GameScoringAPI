using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

// TODO: remove this class?

public class MatchInterceptor : SaveChangesInterceptor
{
    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        UpdateMatchesCount((GameDBContext)eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        UpdateMatchesCount((GameDBContext)eventData.Context);
        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    // TODO: This is not causing any errors, and its half working, but somehow, whenever I try to force the context to Save()
    //      it seems to call the UpdateMatchesCount recursivelly. So for now, Its calculating the count and setting it to the game. 
    private async void UpdateMatchesCount(GameDBContext context)
    {
        if (context == null) return;

        // Get distinct game ids from match entries
        var gameIds = context.ChangeTracker.Entries<Match>()
                                            .Select(e => e.Entity.GameId)
                                            .Distinct()
                                            .ToList();

        foreach (var gameId in gameIds)
        {
            var game = context.Games.Find(gameId);
            if (game != null)
            {
                // Update match count for the game
                var matchCount = context.Set<Match>().Count(m => m.GameId == gameId);
                game.MatchesCount = matchCount;
                context.Update<Game>(game); // TODO: This game entity is never updated in the Database.
                context.Entry(game).State = EntityState.Modified;
                //await context.SaveChangesAsync();
                Console.WriteLine($"GameId: {gameId}, MatchesCount: {matchCount}"); // Logging for debugging
            }
            else
            {
                Console.WriteLine($"Game with Id {gameId} not found.");
            }
        }

    }
}