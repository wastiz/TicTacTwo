namespace DAL;

public record GameConfiguration()
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public int BoardSizeWidth { get; set; } = 5;
    public int BoardSizeHeight { get; set; } = 5;
    public int MovableBoardWidth { get; set; } = 3;
    public int MovableBoardHeight { get; set; } = 3;
    public int[] ChipsCount { get; set; } = new int[] { 0, 4, 4 };
    public int WinCondition { get; set; } = 3; //pieces in row to win
    public int OptionsAfterNMoves { get; set; } = 2;

    public override string ToString() =>
        $"Configuration Name: {Name}\n" +
        $"Board Size: {BoardSizeWidth}x{BoardSizeHeight}\n" +
        $"Movable Board Size: {MovableBoardWidth}x{MovableBoardHeight}\n" +
        $"Chips Count:\n\tPlayer 1 - {ChipsCount[0]}\n\tPlayer 2 - {ChipsCount[1]}\n" +
        $"Win Condition: {WinCondition} pieces in a row\n" +
        $"Options after {OptionsAfterNMoves} moves\n";
}