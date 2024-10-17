namespace GameBrain;

public record struct GameConfiguration()
{
    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;
    public int MovableBoardWidth { get; set; } = 3;
    public int MovableBoardHeight { get; set; } = 3;
    
    //public List<int> ChipsCount { get; set; } = new List<int> { 4, 4 };
    public int[] ChipsCount { get; set; } = new int[] { 4, 4 };
    
    public int WinCondition { get; set; } = 3; //pieces in row to win
    public int MovePieceAfterNMoves { get; set; } = 0; //0 to disable

    public override string ToString() =>
        $"Configuration Name: {Name}\n" +
        $"Board Size: {BoardSizeWidth}x{BoardSizeHeight}\n" +
        $"Movable Board Size: {MovableBoardWidth}x{MovableBoardHeight}\n" +
        $"Chips Count:\n\tPlayer 1 - {ChipsCount[0]}\n\tPlayer 2 - {ChipsCount[1]}\n" +
        $"Win Condition: {WinCondition} pieces in a row\n" +
        $"Move piece after {MovePieceAfterNMoves} moves\n";
}
