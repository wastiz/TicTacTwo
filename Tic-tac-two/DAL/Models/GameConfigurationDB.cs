namespace DAL;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

public class GameConfigurationDB
{
    public int Id { get; set; }  // Primary key for EF Core

    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;
    public int MovableBoardWidth { get; set; } = 3;
    public int MovableBoardHeight { get; set; } = 3;

    // Serialized JSON string for ChipsCount
    public string ChipsCountSerialized { get; set; } = JsonSerializer.Serialize(new int[] { 0, 4, 4 });

    [NotMapped]  // Not mapped to database, but deserialized from ChipsCountSerialized
    public int[] ChipsCount
    {
        get => JsonSerializer.Deserialize<int[]>(ChipsCountSerialized) ?? new int[] { 0, 4, 4 };
        set => ChipsCountSerialized = JsonSerializer.Serialize(value);
    }

    public int WinCondition { get; set; } = 3; // Pieces in row to win
    public int MovePieceAfterNMoves { get; set; } = 0; // 0 to disable

    public override string ToString() =>
        $"Configuration Name: {Name}\n" +
        $"Board Size: {BoardSizeWidth}x{BoardSizeHeight}\n" +
        $"Movable Board Size: {MovableBoardWidth}x{MovableBoardHeight}\n" +
        $"Chips Count:\n\tPlayer 1 - {ChipsCount[0]}\n\tPlayer 2 - {ChipsCount[1]}\n" +
        $"Win Condition: {WinCondition} pieces in a row\n" +
        $"Move piece after {MovePieceAfterNMoves} moves\n";
}
