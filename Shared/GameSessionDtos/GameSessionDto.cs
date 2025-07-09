using Domain;
using Shared.GameConfigDtos;
using Shared.GameStateDtos;

namespace Shared.GameSessionDtos;

public class GameSessionDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public GameConfigDto? GameConfiguration { get; set; }
    public GameStateDto? GameState { get; set; }
    public string? Player1Id { get; set; } = null;
    public string? Player1Username { get; set; } = null;
    public string? Player2Id { get; set; } = null;
    public string? Player2Username { get; set; } = null;
    public string? GameMode { get; set; }
    public string? GamePassword { get; set; }
    public GameStatus GameStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastSaveAt { get; set; }
}
