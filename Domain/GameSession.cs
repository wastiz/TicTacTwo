using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

using System.ComponentModel.DataAnnotations;

public class GameSession
{
    public GameSession()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Autosave " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        GamePassword = Id;
        CreatedAt = DateTime.UtcNow;
        LastSaveAt = DateTime.UtcNow;
    }

    [Key] public string Id { get; set; }
    public string Name { get; set; }
    
    [ForeignKey(nameof(GameConfigId))] 
    public string? GameConfigId { get; set; } = null!;
    public GameConfiguration GameConfiguration { get; set; }

    [ForeignKey(nameof(GameStateId))] 
    public string? GameStateId { get; set; } = null!;
    public GameState? GameState { get; set; } = null!;

    [ForeignKey(nameof(Player1Id))]  
    public string? Player1Id { get; set; } = null!;
    public User Player1 { get; set; }

    [ForeignKey(nameof(Player2Id))] 
    public string? Player2Id { get; set; } = null!;
    public User Player2 { get; set; }

    public string? GameMode { get; set; } = null!;
    public string? GamePassword { get; set; }

    public GameStatus GameStatus { get; set; } = GameStatus.InProgress;
    public DateTime CreatedAt { get; set; }
    public DateTime LastSaveAt { get; set; }
}


public enum GameStatus
{
    InProgress,
    Draw,
    Player1Won,
    Player2Won
}