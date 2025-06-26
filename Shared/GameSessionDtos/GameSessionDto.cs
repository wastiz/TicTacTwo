using Domain;

namespace Shared.GameSessionDtos;

public class GameSessionDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public GameConfiguration? GameConfiguration { get; set; }
    public GameState? GameState { get; set; }
    public string? Player1Id { get; set; }
    public string? Player1Username { get; set; }
    public string? Player2Id { get; set; } = null;
    public string? Player2Username { get; set; } = null;
    public string? GameMode { get; set; }
    public string? GamePassword { get; set; }
    public GameStatus GameStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastSaveAt { get; set; }
}
