using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Game
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string GameName { get; set; }
    public string? GameDescription { get; set; }
    public int MinPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public int AverageDuration { get; set; }

    public ICollection<Match> Matches { get; set; }  // Navigation property to Matches
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