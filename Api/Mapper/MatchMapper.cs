
namespace GameScoringAPI.Mapper;

public static class MatchMapper
{
    public static MatchForMatchDto MapToDTO(Match match, bool? includeDataPoints)
    {
        var matchDto = new MatchForMatchDto
        {
            MatchId = match.Id,
            GameId = match.GameId,
            MatchDate = match.MatchDate,
            Notes = match.Notes,
            isFinished = match.isFinished,
            PlayerCount = match.PlayerCount
        };

        if (includeDataPoints == true)
        {
            matchDto.MatchDataPoints = match.MatchDataPoints.Select(dp => new MatchDataPointForMatchDto
            {
                Id = dp.Id,
                PlayerName = dp.PlayerName,
                GamePoints = dp.GamePoints,
                PointsDescription = dp.PointsDescription,
                CreatedDate = dp.CreatedDate
            }).ToList();
        }
        else
        {
            matchDto.MatchDataPoints = new List<MatchDataPointForMatchDto>();
        }

        return matchDto;
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