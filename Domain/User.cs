using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required] public string Username { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
    public int GamesPlayed { get; set; } = 0;
    public int GamesWon { get; set; } = 0;
    public int GamesLost { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
