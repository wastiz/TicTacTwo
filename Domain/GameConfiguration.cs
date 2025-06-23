namespace DAL;
using System.Text.Json;
using System.ComponentModel.DataAnnotations.Schema;

public class  GameConfiguration
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;
    public int MovableBoardWidth { get; set; } = 3;
    public int MovableBoardHeight { get; set; } = 3;
    public int Player1Chips { get; set; } = 4;
    public int Player2Chips { get; set; } = 4;
    public int WinCondition { get; set; } = 3; // Pieces in row to win
    public int OptionsAfterNMoves { get; set; } = 2; // 0 to instantly unlock options
    [ForeignKey(nameof(CreatedBy))] public string? CreatedBy { get; set; } = null!;
    public User User { get; set; }
    public override string ToString() =>
        $"Configuration Name: {Name}\n" +
        $"Board Size: {BoardSizeWidth}x{BoardSizeHeight}\n" +
        $"Movable Board Size: {MovableBoardWidth}x{MovableBoardHeight}\n" +
        $"Chips Count:\n\tPlayer 1 - {Player1Chips}\n\tPlayer 2 - {Player2Chips}\n" +
        $"Win Condition: {WinCondition} pieces in a row\n" +
        $"Move piece after {OptionsAfterNMoves} moves\n";
}
