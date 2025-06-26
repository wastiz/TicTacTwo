using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

using System.ComponentModel.DataAnnotations;

public class GameSession
{
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }

    [ForeignKey("GameConfiguration")]
    public string GameConfigId { get; set; }

    public GameConfiguration GameConfiguration { get; set; }

    [ForeignKey("GameState")]
    public string? GameStateId { get; set; }
    public GameState? GameState { get; set; }

    [ForeignKey("Player1")]
    public string? Player1Id { get; set; }
    public User Player1 { get; set; }

    [ForeignKey("Player2")]
    public string? Player2Id { get; set; }
    public User Player2 { get; set; }

    public string? GameMode { get; set; }
    public string? GamePassword { get; set; }

    public GameStatus GameStatus { get; set; } = GameStatus.InProgress;
    public DateTime CreatedAt { get; set; }
    public DateTime LastSaveAt { get; set; }

    public GameSession()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Autosave " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        GamePassword = Id;
        CreatedAt = DateTime.UtcNow;
        LastSaveAt = DateTime.UtcNow;
    }
}



public enum GameStatus
{
    InProgress,
    Draw,
    Player1Won,
    Player2Won
}