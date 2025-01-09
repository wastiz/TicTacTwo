using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameSession> GameSessions { get; set; } = default!;
        public DbSet<GameConfiguration> GameConfigurations { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext>? options = null)
            : base(options ?? new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={FileHelper.BasePath}app.db")
                .Options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<GameSession>(entity =>
            {
                entity.HasKey(gs => gs.Id);

                entity.HasOne(gs => gs.GameConfiguration)
                      .WithMany()
                      .HasForeignKey(gs => gs.GameConfigId)
                      .IsRequired();

                entity.HasOne(gs => gs.Player1)
                      .WithMany()
                      .HasForeignKey(gs => gs.Player1Id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(gs => gs.Player2)
                      .WithMany()
                      .HasForeignKey(gs => gs.Player2Id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.OwnsOne(gs => gs.GameState, gs =>
                {
                    gs.Property(g => g.BoardJson).HasColumnName("BoardJson");
                    gs.Property(g => g.ChipsLeftJson).HasColumnName("ChipsLeftJson");
                    gs.Property(g => g.PlayersMovesJson).HasColumnName("PlayersMovesJson");
                });
            });
            
            modelBuilder.Entity<GameConfiguration>(entity =>
            {
                entity.HasKey(gc => gc.Id);
            });
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
            });
        }
    }
}
