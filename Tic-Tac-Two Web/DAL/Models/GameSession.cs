using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

using System.ComponentModel.DataAnnotations;

public class GameSession
{
    [Key] public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public string Name { get; set; } = "Autosave " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public string? GameConfigId { get; set; } = null!;
    [ForeignKey(nameof(GameConfigId))] public GameConfiguration GameConfiguration { get; set; }
    public string? GameStateId { get; set; } = null!;
    [ForeignKey(nameof(GameStateId))] public GameState? GameState { get; set; } = null!;
    public string? Player1Id { get; set; } = null!;
    [ForeignKey(nameof(Player1Id))] public User Player1 { get; set; }
    public string? Player2Id { get; set; } = null!;
    [ForeignKey(nameof(Player2Id))] public User Player2 { get; set; }
    public string? GameMode { get; set; } = null!;
    public string? GamePassword { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime LastSaveAt { get; set; } = DateTime.UtcNow;

}