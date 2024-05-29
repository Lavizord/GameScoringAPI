using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

public class MatchDataPoint
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int MatchId { get; set; }

    public string PlayerName { get; set; }
    public int GamePoints { get; set; }
    public string PointsDescription { get; set; }
    public DateTime CreatedDate { get; set; }

    [ForeignKey("MatchId")]
    public Match Match { get; set; }  // Navigation property to Match
}

public class MatchDataPointDto
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string GameName { get; set; }  // Include GameName in DTO
    public int MatchId { get; set; }
    public bool isMatchFinished { get; set; }
    public string PlayerName { get; set; }
    public int GamePoints { get; set; }
    public string PointsDescription { get; set; }
    public DateTime CreatedDate { get; set; }
}