namespace DAL;

public record GameSession
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = "Autosave " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public string GameMode { get; set; } = default!;
    public GameConfiguration GameConfig { get; set; } = default!;
    public GameState? GameState { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime LastSaveAt { get; set; } = DateTime.Now;
}