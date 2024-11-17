using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<GameStateDB> GameStates { get; set; } = default!;
    public DbSet<GameConfigurationDB> GameConfigurations { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext>? options = null) 
        : base(options ?? new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source={connectionString}")
            .Options)
    {
    }
}