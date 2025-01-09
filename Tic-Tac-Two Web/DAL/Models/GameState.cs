using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace DAL;

using System.ComponentModel.DataAnnotations;

public class GameState
{
    public string BoardJson { get; set; } = default!;
    public string ChipsLeftJson { get; set; } = default!;
    public string PlayersMovesJson { get; set; } = default!;

    [NotMapped]
    public int[,] Board
    {
        get => JsonSerializer.Deserialize<int[,]>(BoardJson);
        set => BoardJson = JsonSerializer.Serialize(value);
    }

    [NotMapped]
    public int[] ChipsLeft
    {
        get => JsonSerializer.Deserialize<int[]>(ChipsLeftJson);
        set => ChipsLeftJson = JsonSerializer.Serialize(value);
    }
    [NotMapped]
    public int[] PlayersMoves
    {
        get => JsonSerializer.Deserialize<int[]>(PlayersMovesJson);
        set => PlayersMovesJson = JsonSerializer.Serialize(value);
    }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int PlayerNumber { get; set; } = default!;
    public bool Player1Options { get; set; } = default!;
    public bool Player2Options { get; set; } = default!;
    public int Win { get; set; } = default!;
}