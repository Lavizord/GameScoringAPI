
public static class GameValidation
{
    public static IResult IsGameValid(Game game)
    {
        if (string.IsNullOrWhiteSpace(game.GameName))
            return Results.BadRequest("GameName can't be empty.");

        // Player count validations.
        if(int.IsNegative(game.MinPlayers))
            return Results.BadRequest("MinPlayers can't be negative.");
        if(int.IsNegative(game.MaxPlayers))
            return Results.BadRequest("MaxPlayers can't be negative.");
            
        // Here we want to allow the game to be valid.
        if(game.MaxPlayers <= game.MinPlayers && game.MinPlayers != game.MaxPlayers)
            return Results.BadRequest("MinPlayers can't be equal to or bigger than MaxPlayers.");

        // Game Duration validations
        if(game.AverageDuration < 0)
            return Results.BadRequest("AverageDuration can't negative.");


        return Results.Accepted(); 
    }
}
