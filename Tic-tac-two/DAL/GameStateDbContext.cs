namespace DAL;

using Microsoft.EntityFrameworkCore;

public class GameStateDbContext : DbContext
{
    public DbSet<GameStateDB> GameStates { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=game.db");
    }
}