namespace DAL;

using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class GameStateDB
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string GameConfig { get; set; } = default!;
    public string Board { get; set; } = default!;
    public string ChipsLeft { get; set; } = default!;
    public string PlayersMoves { get; set; } = default!;
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int PlayerNumber { get; set; } = default!;
    public bool Player1Options { get; set; } = default!;
    public bool Player2Options { get; set; } = default!;
    public int Win { get; set; } = default!;
}