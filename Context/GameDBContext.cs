using Microsoft.EntityFrameworkCore;

public class GameDBContext : DbContext
{
    public DbSet<MatchDataPoint> MatchDataPoints { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Match> Matches { get; set; }

    public GameDBContext(DbContextOptions<GameDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MatchDataPoint>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasOne<Match>()
                .WithMany(m => m.MatchDataPoints)
                .HasForeignKey(e => e.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.HasOne(m => m.Game)
                .WithMany(g => g.Matches)
                .HasForeignKey(m => m.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}