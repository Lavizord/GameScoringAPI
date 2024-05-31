public static class SqlTriggers
{
    public const string MatchesTriggerInsert = @"
       -- Drop the trigger if it exists
        DROP TRIGGER IF EXISTS insert_game_match_count;

        -- Create the trigger
        CREATE TRIGGER insert_game_match_count
        AFTER INSERT ON matches
        BEGIN
            UPDATE GAMES
            SET MatchCount = (
                SELECT COUNT(*)
                FROM matches
                WHERE matches.GameID = COALESCE(NEW.GameID, OLD.GameID)
            )
            WHERE Id = COALESCE(NEW.GameID, OLD.GameID);
        END;
    ";
    
    public const string MatchesTriggerDelete = @"
       -- Drop the trigger if it exists
        DROP TRIGGER IF EXISTS delete_game_match_count;

        -- Create the trigger
        CREATE TRIGGER delete_game_match_count
        AFTER DELETE ON matches
        BEGIN
            UPDATE GAMES
            SET MatchCount = (
                SELECT COUNT(*)
                FROM matches
                WHERE matches.GameID = COALESCE(NEW.GameID, OLD.GameID)
            )
            WHERE Id = COALESCE(NEW.GameID, OLD.GameID);
        END;
    ";
    
    // TODO: Finish this and then run migration.
    public const string MatchesDataPointTriggerDelete = @"
       -- Drop the trigger if it exists
        DROP TRIGGER IF EXISTS delete_match_player_count;

        -- Create the trigger
        CREATE TRIGGER delete_match_player_count
        AFTER DELETE ON MatchDataPoints
        BEGIN
            UPDATE MATCHES
            SET PlayerCount = (
                SELECT COUNT(DISTINCT PlayerName)
                FROM MatchDataPoints
                WHERE matches.Id = COALESCE(NEW.MatchId, OLD.MatchId)
            )
            WHERE Id = COALESCE(NEW.MatchId, OLD.MatchId);
        END;
    ";
    public const string MatchesDataPointTriggerInsert = @"
       -- Drop the trigger if it exists
        DROP TRIGGER IF EXISTS insert_match_player_count;

        -- Create the trigger
        CREATE TRIGGER insert_match_player_count
        AFTER INSERT ON MatchDataPoints
        BEGIN
            UPDATE MATCHES
            SET PlayerCount = (
                SELECT COUNT(DISTINCT PlayerName)
                FROM MatchDataPoints
                WHERE matches.Id = COALESCE(NEW.MatchId, OLD.MatchId)
            )
            WHERE Id = COALESCE(NEW.MatchId, OLD.MatchId);
        END;
    ";
}