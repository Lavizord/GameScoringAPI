using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Game
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Auto-incrementing primary key

    public string GameName { get; set; }
    public string GameDescription { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int AverageDuration { get; set; }

    // Navigation property for related MatchDataPoints
    public List<MatchDataPoint> MatchDataPoints { get; set; } = new();

    // Navigation property for related Matches
    public List<Match> Matches { get; set; } = new();
}

public class GameDto
{
    public int Id { get; set; }  // Add the Id property
    public string GameName { get; set; }
    public string GameDescription { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int AverageDuration { get; set; }
}

public class GameWithMatchDataPointDto
{
    public int Id { get; set; }
    public string GameName { get; set; }
    public string GameDescription { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int AverageDuration { get; set; }
    public List<MatchDto> Matches { get; set; } = new List<MatchDto>();
}