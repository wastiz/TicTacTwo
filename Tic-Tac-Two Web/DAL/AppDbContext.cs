using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameSession> GameSessions { get; set; } = default!;
        public DbSet<GameConfiguration> GameConfigurations { get; set; } = default!;
        public DbSet<GameState> GameStates { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext>? options = null)
            : base(options ?? new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={FileHelper.BasePath}app.db")
                .Options)
        {
        }
    }
}
