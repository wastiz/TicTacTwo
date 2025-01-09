namespace DAL;

public record GameState()
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public int[][] Board { get; set; } = default!;
    public int GridX { get; set; } = default!;
    public int GridY { get; set; } = default!;
    public int[] ChipsLeft { get; set; } = default!;
    public int[] PlayersMoves { get; set; } = default!;
    public int PlayerNumber { get; set; } = default!;
    public bool Player1Options { get; set; } = default!;
    public bool Player2Options { get; set; } = default!;
    public int Win = default!;

    public override string ToString() =>
        $"Board: {Board}\n" +
        $"GridX: {GridX}\n" +
        $"GridY: {GridY}\n" +
        $"ChipsLeft: {ChipsLeft}\n" +
        $"PlayerNumber: {PlayerNumber}";
}