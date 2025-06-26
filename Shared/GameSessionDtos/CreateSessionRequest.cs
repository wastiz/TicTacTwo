namespace Shared.GameSessionDtos;

public class CreateSessionRequest
{
    public string ConfigId { get; set; } = default!;
    public string GameMode { get; set; } = default!;
    public string? Password { get; set; }
}