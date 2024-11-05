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

    [NotMapped]
    public GameConfiguration GameConfig
    {
        get => JsonSerializer.Deserialize<GameConfiguration>(GameConfigJson) is GameConfiguration config
            ? config
            : new GameConfiguration();
        set => GameConfigJson = JsonSerializer.Serialize(value);
    }


    public string BoardJson { get; set; } = default!;

    [NotMapped]
    public int[][] Board
    {
        get => JsonSerializer.Deserialize<int[][]>(BoardJson) ?? new int[0][];
        set => BoardJson = JsonSerializer.Serialize(value);
    }

    public string ChipsLeftJson { get; set; } = default!;

    [NotMapped]
    public int[] ChipsLeft
    {
        get => JsonSerializer.Deserialize<int[]>(ChipsLeftJson) ?? Array.Empty<int>();
        set => ChipsLeftJson = JsonSerializer.Serialize(value);
    }

    public int GridX { get; set; }
    public int GridY { get; set; }
    public int PlayerNumber { get; set; } = 1;
}