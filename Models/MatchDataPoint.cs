using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class MatchDataPoint
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; } // Auto-incrementing primary key

    public int GameId { get; set; } // Foreign key referencing the Game table

    public int MatchId { get; set; }

    public string PlayerName { get; set; }
    public int GamePoints { get; set; }
    public string PointsDescription { get; set; }
}

public class MatchDataPointDto
{
    public int GameId { get; set; }
    public int MatchId { get; set; }
    public string PlayerName { get; set; }
    public int GamePoints { get; set; }
    public string PointsDescription { get; set; }
}