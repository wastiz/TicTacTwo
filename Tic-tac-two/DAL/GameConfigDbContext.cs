namespace DAL;

using Microsoft.EntityFrameworkCore;

public class GameConfigDbContext : DbContext
{
    public DbSet<GameConfigurationDB> GameConfigurations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=gameConfigurations.db");
    }
}