namespace Shared;

public class GameMode
{
    public string Value { get; init; } = default!;
    public string Label { get; init; } = default!;
    public bool Disabled { get; init; }
}

public static class GameModes
{
    public static readonly IReadOnlyList<GameMode> All = new List<GameMode>
    {
        new() { Value = "two-players", Label = "Local Game" },
        new() { Value = "two-players-online", Label = "Online Game (Beta)" },
        new() { Value = "vs-ai", Label = "vs AI (broken, bot cheats)" },
        new() { Value = "ai-vs-ai", Label = "AI vs AI (coming soon)", Disabled = true }
    };
}