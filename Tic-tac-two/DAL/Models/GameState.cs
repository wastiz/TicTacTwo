namespace DAL;

public record struct GameState()
{
    public string StateName { get; set; } = default!;
    public GameConfiguration GameConfig { get; set; } = default!;
    public int[][] Board { get; set; } = default!;
    public int GridX { get; set; } = default!;
    public int GridY { get; set; } = default!;
    public int[] ChipsLeft { get; set; } = default!;
    public int PlayerNumber { get; set; } = 1;

    public override string ToString() =>
        $"State Name: {StateName}\n" +
        $"Config Name: {GameConfig}\n" +
        $"Board: {Board}\n" +
        $"GridX: {GridX}\n" +
        $"GridY: {GridY}\n" +
        $"ChipsLeft: {ChipsLeft}\n" +
        $"PlayerNumber: {PlayerNumber}";
}