namespace Shared.GameConfigDtos;

public class GameConfig
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
}