using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameSession> GameSessions { get; set; } = default!;
        public DbSet<GameConfiguration> GameConfigurations { get; set; } = default!;
        public DbSet<GameState> GameStates { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameConfiguration>().HasData(
                new GameConfiguration
                {
                    Id = "classic",
                    Name = "Classical"
                },
                new GameConfiguration
                {
                    Id = "big-game",
                    Name = "Big Game",
                    BoardSizeWidth = 10,
                    BoardSizeHeight = 10,
                    MovableBoardWidth = 5,
                    MovableBoardHeight = 5,
                    Player1Chips = 6,
                    Player2Chips = 6,
                    WinCondition = 3,
                    AbilitiesAfterNMoves = 3
                }
            );
        }
    }
}
