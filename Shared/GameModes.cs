namespace Shared;

public class GameMode
{
    public string Value { get; set; } = default!;
    public string Label { get; set; } = default!;
}


public static class GameModes
{
    public static readonly List<GameMode> All = new()
    {
        new GameMode { Value = "two-players", Label = "Local Game" },
        new GameMode { Value = "two-players-online", Label = "Online Game" },
        new GameMode { Value = "vs-ai", Label = "vs AI (coming soon)" },
        new GameMode { Value = "ai-vs-ai", Label = "AI vs AI (coming soon)" }
    };
}