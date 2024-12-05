namespace DAL;

using System.Text.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class GameStateDB
{
    [Key]
    public int Id { get; set; }
    public string StateName { get; set; } = default!;
    public string GameConfigJson { get; set; } = default!;
    public string BoardJson { get; set; } = default!;
    public string ChipsLeftJson { get; set; } = default!;
    public string PlayersMovesJson { get; set; } = default!;
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int PlayerNumber { get; set; } = default!;
    public bool Player1Options { get; set; } = default!;
    public bool Player2Options { get; set; } = default!;
    public int Win { get; set; } = default!;
}