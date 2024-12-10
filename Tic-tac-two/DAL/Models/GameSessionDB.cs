using System.ComponentModel.DataAnnotations.Schema;

namespace DAL;

using System.ComponentModel.DataAnnotations;

public class GameSessionDB
{
    [Key]
    public string Id { get; set; }
    
    public string GameStateId { get; set; }
    [ForeignKey(nameof(GameStateId))]
    public GameStateDB GameState { get; set; }
    
    public string Player1Id { get; set; }
    [ForeignKey(nameof(Player1Id))]
    public User Player1 { get; set; }
    
    public string Player2Id { get; set; }
    [ForeignKey(nameof(Player2Id))]
    public User Player2 { get; set; }
}