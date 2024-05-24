using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure SQLite
builder.Services.AddDbContext<GameDBContext>(options =>
    options.UseSqlite("Data Source=games.db"));

var app = builder.Build();



// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseHttpsRedirection();


// Create SINGLE Game endpoint
app.MapPost("/game", async (GameDto gameDto, GameDBContext context) =>
{
    var game = new Game
    {
        GameName = gameDto.GameName,
        GameDescription = gameDto.GameDescription,
        MinPlayers = gameDto.MinPlayers,
        MaxPlayers = gameDto.MaxPlayers,
        AverageDuration = gameDto.AverageDuration
    };

    context.Games.Add(game);
    await context.SaveChangesAsync();

    return Results.Created($"/games/{game.Id}", game);
})
.WithName("PostGame")
.WithOpenApi();

app.MapPut("/game/{id}", async (int id, GameDto gameDto, GameDBContext context) =>
{
    // Retrieve the existing game
    var existingGame = await context.Games.FindAsync(id);
    if (existingGame == null)
    {
        return Results.NotFound($"Game with ID {id} not found.");
    }

    // Update the game properties
    existingGame.GameName = gameDto.GameName;
    existingGame.GameDescription = gameDto.GameDescription;
    existingGame.MinPlayers = gameDto.MinPlayers;
    existingGame.MaxPlayers = gameDto.MaxPlayers;
    existingGame.AverageDuration = gameDto.AverageDuration;

    // Save changes to the database
    await context.SaveChangesAsync();

    // Return a 200 OK response with the updated game
    return Results.Ok(existingGame);
})
.WithName("PutGame")
.WithOpenApi();


// Create MatchDataPoint endpoint
app.MapPost("/match-data-point", async (MatchDataPointDto dataPointDto, GameDBContext context) =>
{
    var gameExists = await context.Games.AnyAsync(g => g.Id == dataPointDto.GameId);
    if (!gameExists)
    {
        return Results.NotFound($"Game with ID {dataPointDto.GameId} not found.");
    }

    var matchExists = await context.Matches.AnyAsync(m => m.Id == dataPointDto.MatchId);
    if (!matchExists)
    {
        return Results.NotFound($"Match with ID {dataPointDto.MatchId} not found.");
    }

    var matchDataPoint = new MatchDataPoint
    {
        GameId = dataPointDto.GameId,
        MatchId = dataPointDto.MatchId,
        PlayerName = dataPointDto.PlayerName,
        GamePoints = dataPointDto.GamePoints,
        PointsDescription = dataPointDto.PointsDescription
    };

    context.MatchDataPoints.Add(matchDataPoint);
    await context.SaveChangesAsync();

    return Results.Created($"/match-data-points/{matchDataPoint.Id}", matchDataPoint);
})
.WithName("PostMatchDataPoint")
.WithOpenApi();


// Create multiple MatchDataPoints endpoint
app.MapPost("/match-multiple-data-points", async (List<MatchDataPointDto> dataPointDtos, GameDBContext context) =>
{
    // Check if all the games and matches exist
    var gameIds = dataPointDtos.Select(dp => dp.GameId).Distinct();
    var matchIds = dataPointDtos.Select(dp => dp.MatchId).Distinct();

    var gamesExist = await context.Games.Where(g => gameIds.Contains(g.Id)).Select(g => g.Id).ToListAsync();
    if (gamesExist.Count != gameIds.Count())
    {
        return Results.NotFound("One or more games not found.");
    }

    var matchesExist = await context.Matches.Where(m => matchIds.Contains(m.Id)).Select(m => m.Id).ToListAsync();
    if (matchesExist.Count != matchIds.Count())
    {
        return Results.NotFound("One or more matches not found.");
    }

    // Create the list of MatchDataPoint instances
    var matchDataPoints = dataPointDtos.Select(dataPointDto => new MatchDataPoint
    {
        GameId = dataPointDto.GameId,
        MatchId = dataPointDto.MatchId,
        PlayerName = dataPointDto.PlayerName,
        GamePoints = dataPointDto.GamePoints,
        PointsDescription = dataPointDto.PointsDescription
    }).ToList();

    // Add the MatchDataPoints to the database
    context.MatchDataPoints.AddRange(matchDataPoints);
    await context.SaveChangesAsync();

    // Return a 201 Created response with the created MatchDataPoints
    return Results.Created("/match-data-points", matchDataPoints);
})
.WithName("PostMultipleMatchDataPoints")
.WithOpenApi();


// Get Game with Matches and Match Data Points endpoint
app.MapGet("/games-with-matches/{GameId}", async (int id, GameDBContext context) =>
{
    var game = await context.Games
        .Include(g => g.Matches)
        .ThenInclude(m => m.MatchDataPoints)
        .FirstOrDefaultAsync(g => g.Id == id);

    if (game == null)
    {
        return Results.NotFound();
    }

    var gameWithMatchesDto = new GameWithMatchDataPointDto
    {
        Id = game.Id,
        GameName = game.GameName,
        GameDescription = game.GameDescription,
        MinPlayers = game.MinPlayers,
        MaxPlayers = game.MaxPlayers,
        AverageDuration = game.AverageDuration,
        MatchDataPoints = game.Matches.SelectMany(m => m.MatchDataPoints.Select(dp => new MatchDataPointDto
        {
            GameId = dp.GameId,
            MatchId = dp.MatchId,
            PlayerName = dp.PlayerName,
            GamePoints = dp.GamePoints,
            PointsDescription = dp.PointsDescription
        })).ToList()
    };

    return Results.Ok(gameWithMatchesDto);
})
.WithName("GetGameWithMatchesById")
.WithOpenApi();


app.MapMatchEndpoints();
app.Run();