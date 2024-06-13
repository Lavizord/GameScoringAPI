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
            SET MatchesCount = (
                SELECT COUNT(*)
                FROM matches
                WHERE matches.GameID = NEW.GameID
            )
            WHERE Id = NEW.GameID;
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
            SET MatchesCount = (
                SELECT COUNT(*)
                FROM matches
                WHERE matches.GameID = OLD.GameID
            )
            WHERE Id = OLD.GameID;
        END;
    ";
    
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
                WHERE matches.Id = OLD.MatchId
            )
            WHERE Id = OLD.MatchId;
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
                WHERE matches.Id = NEW.MatchId
            )
            WHERE Id = NEW.MatchId;
        END;
    ";
}