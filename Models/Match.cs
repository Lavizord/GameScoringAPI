using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class Match
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Auto-incrementing primary key

    public int GameId { get; set; } // Foreign key referencing the Game table

    public DateTime MatchDate { get; set; }
    public string Notes { get; set; }

    // Navigation property for related Game
    public Game Game { get; set; }

    // Navigation property for related MatchDataPoints
    public List<MatchDataPoint> MatchDataPoints { get; set; } = new();
}

public class MatchDto
{
    public int GameId { get; set; }
    public DateTime MatchDate { get; set; }
    public string Notes { get; set; }
}