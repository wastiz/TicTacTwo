namespace DAL;

using Microsoft.EntityFrameworkCore;

public class GameConfigDbContext : DbContext
{
    public DbSet<GameConfigurationDB> GameConfigurations { get; set; }
    public GameConfigDbContext(DbContextOptions<GameConfigDbContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=gameConfigurations.db");
    }
}