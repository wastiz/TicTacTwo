namespace Shared.GameStateDtos;

public class GameStateDto
{
    public string? Id { get; set; } = null;
    public int[][] Board { get; set; } = default!;
    public int[] ChipsLeft { get; set; } = default!;
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int PlayerNumber { get; set; }
    public bool Player1Abilities { get; set; }
    public bool Player2Abilities { get; set; }
    public int Win { get; set; }
    public string? Message { get; set; } = null;
}
