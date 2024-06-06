
namespace GameScoringAPI.Mapper;


/// <summary>
/// CLass to agregate Mapping of Match Entities to severel Match related DTOs. 
/// Query Objects should be provided already filtered, and should be resolved after the output of these methods.
/// </summary>
public static class MatchMapper
{
    /// <summary>
    /// Method used the .Select() method of the IQueryable class to map the Match model to the MatchDto, 
    /// if we provide the includeDataPoints as true, it also resolves and maps the MatchDataPoint
    /// entity to the MatchDataPointforMatchDto.
    /// </summary>
    /// <param name="match">The IQueryable<Match> of the database model, to be selected into thhe DTO.</param>
    /// <param name="includeDataPoints"> If we shouuld include the child objects includeDataPoints when doing the Select.</param>
    /// <returns></returns>
    public static IQueryable<MatchForMatchDto> MapToDTO(IQueryable<Match>? match, bool? includeDataPoints)
    {   
          return match
                .Select(m => new MatchForMatchDto
                {
                    MatchId = m.Id,
                    GameId = m.GameId,
                    MatchDate = m.MatchDate,
                    Notes = m.Notes,
                    isFinished = m.isFinished,
                    PlayerCount = m.PlayerCount,
                    MatchDataPoints = includeDataPoints == true
                        ? m.MatchDataPoints.Select(dp => new MatchDataPointForMatchDto
                        {
                            Id = dp.Id,
                            PlayerName = dp.PlayerName,
                            GamePoints = dp.GamePoints,
                            PointsDescription = dp.PointsDescription,
                            CreatedDate = dp.CreatedDate
                        }).ToList()
                        : new List<MatchDataPointForMatchDto>()
                });
    }
/*
    public static MatchDetailsDTO MapToDetailsDTO(Match match, Game game, List<MatchDataPoint> matchDataPoints)
    {
        return new MatchDetailsDTO
        {
            MatchId = match.Id,
            GameId = match.GameId,
            MatchDate = match.MatchDate,
            Notes = match.Notes,
            PlayerCount = match.PlayerCount,
            IsFinished = match.IsFinished,
            Game = new GameDTO
            {
                GameId = game.Id,
                Name = game.Name
            },
            MatchDataPoints = matchDataPoints.Select(dp => new MatchDataPointDTO
            {
                DataPointId = dp.Id,
                MatchId = dp.MatchId,
                PlayerName = dp.PlayerName,
                GamePoints = dp.GamePoints
            }).ToList()
        };
    }
*/
}