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
                .UseNpgsql(GetConnectionString())
                .Options)
        {
        }

        private static string GetConnectionString()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            return environment switch
            {
                "Development" => "Host=localhost;Database=tic_tac_two;Username=postgres;Password=admin",
                "Production" => "Host=localhost;Database=tic_tac_two;Username=postgres;Password=PGtitan4321",
                _ => throw new InvalidOperationException($"Unknown environment: {environment}")
            };
        }
    }
}